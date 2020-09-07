//------------------------------------------------------------------------------
//   Copyright (c) Microsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Microsoft.VisualStudio.WebHost
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Web;

    // Singularity
    using Microsoft.SingSharp;
    using Microsoft.SingSharp.Runtime;
    using Microsoft.Singularity.Channels;
    //

    internal sealed class Connection  {
        private Server _server;
        private Socket _socket;

        private static string _localServerIP;
        private static string _clientIP;

        public Connection(Server server, Socket socket, string clientIP) {
            _server = server;
            _socket = socket;
            _clientIP = clientIP;
        }

        public bool Connected {
            get {
                return _socket.Connected;
            }
        }

        public bool IsLocal {
            get {
                string remoteIP = RemoteIP;

                if (remoteIP.Equals("127.0.0.1")) {
                    return true;
                }

                if (_clientIP.Equals(remoteIP)) {
                    return true;
                }

                //return false;
                return true;
            }
        }

        public string! LocalIP {
            get {
                IPEndPoint endPoint = (IPEndPoint)_socket.LocalEndPoint;
                if ((endPoint != null) && (endPoint.Address != null)) {
                    return (!)endPoint.Address.ToString();
                }
                else {
                    return "127.0.0.1";
                }
            }
        }

        public string! RemoteIP {
            get {
                IPEndPoint endPoint = (IPEndPoint)_socket.RemoteEndPoint;
                if ((endPoint != null) && (endPoint.Address != null)) {
                    return (!)endPoint.Address.ToString();
                }
                else {
                    // REVIEW: Is this correct? If RemoteEndPoint is not
                    //         set, we end up returning 127.0.0.1 which is
                    //         treated as localhost, and may not be what
                    //         we want.
                    return "127.0.0.1";
                }
            }
        }

        public void Close() {
            try {
                if (_socket != null) // ROTORTODO
                {
                    _socket.Shutdown(SocketShutdown.Both);
                    _socket.Close();
                }
            }
            catch {
            }
            finally {
                _socket = null;
            }
        }

        private static String! MakeResponseHeaders(int statusCode, String moreHeaders, int contentLength, bool keepAlive) {
            StringBuilder sb = new StringBuilder();

            sb.Append("HTTP/1.1 " + statusCode + " " + HttpWorkerRequest.GetStatusDescription(statusCode) + "\r\n");
            sb.Append("Server: Microsoft VisualStudio .NET WebServer/" + Messages.VersionString + "\r\n");
            sb.Append("Date: " + DateTime.Now.ToUniversalTime().ToString("R") + "\r\n");
            if (contentLength >= 0)
                sb.Append("Content-Length: " + contentLength + "\r\n");
            if (moreHeaders != null)
                sb.Append(moreHeaders);
            if (!keepAlive)
                sb.Append("Connection: Close\r\n");
            sb.Append("\r\n");

            return sb.ToString();
        }

        public byte[] ReadRequestBytes(int maxBytes) {
            try {
                if (WaitForRequestBytes() == 0) {
                    return null;
                }

                int numBytes = _socket.Available;
                if (numBytes > maxBytes)
                    numBytes = maxBytes;

                int numReceived = 0;
                byte[] buffer = new byte[numBytes];

                if (numBytes > 0) {
                    numReceived = _socket.Receive(buffer, 0, numBytes, SocketFlags.None);
                }

                if (numReceived < numBytes) {
                    byte[] tempBuffer = new byte[numReceived];

                    if (numReceived > 0) {
                        Buffer.BlockCopy(buffer, 0, tempBuffer, 0, numReceived);
                    }

                    buffer = tempBuffer;
                }

                return buffer;
            }
            catch {
                return null;
            }
        }

        public void Write100Continue() {
            WriteEntireResponseFromString(100, null, null, true);
        }

        public void WriteBody(byte[] data, int offset, int length) {
            _socket.Send(data, offset, length, SocketFlags.None);
        }

        public void WriteBody([Claims] byte[]! in ExHeap data)
        {
            _socket.Send(data, SocketFlags.None);
        }

        public void WriteEntireResponseFromString(int statusCode, String extraHeaders, String body, bool keepAlive) {
            try {
                int bodyLength = (body != null) ? Encoding.UTF8.GetByteCount(body) : 0;
                string headers = MakeResponseHeaders(statusCode, extraHeaders, bodyLength, keepAlive);

                _socket.Send(Encoding.UTF8.GetBytes(headers + body));
            }
            catch (SocketException) {
            }
            finally {
                if (!keepAlive) {
                    Close();
                }
            }
        }

        public void WriteErrorAndClose(int statusCode, string message) {
            string body = Messages.FormatErrorMessageBody(statusCode, _server.VirtualPath);
            if (message != null && message.Length > 0) {
                body += "\r\n<!--\r\n" + message + "\r\n-->";
            }
            WriteEntireResponseFromString(statusCode, null, body, false);
        }

        public void WriteErrorAndClose(int statusCode) {
            WriteErrorAndClose(statusCode, null);
        }

        public int WaitForRequestBytes() {
            int availBytes = 0;

            try {
                if (_socket.Available == 0) {
                    // poll until there is data
                    _socket.Poll(100000 /* 100ms */, SelectMode.SelectRead);
                    if (_socket.Available == 0 && _socket.Connected) {
                        _socket.Poll(10000000 /* 10sec */, SelectMode.SelectRead);
                    }
                }

                availBytes = _socket.Available;
            }
            catch {
            }

            return availBytes;
        }

        public void WriteHeaders(int statusCode, String extraHeaders) {
            string headers = MakeResponseHeaders(statusCode, extraHeaders, -1, false);

            try {
                _socket.Send(Encoding.UTF8.GetBytes(headers));
            }
            catch (SocketException) {
            }
        }
    }
}
