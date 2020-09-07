///////////////////////////////////////////////////////////////////////////////
//
//  Microsoft Research Singularity / Netstack
//
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//
//  File: TestUDP.cs
//

using System;
using System.Threading;

using System.Net.IP;
using Drivers.Net;
using NetStack.NetDrivers;

namespace NetStack.Runtime.TestPing
{
    public class Receiver
    {
        private UdpSession session;
        private int maxReceivePackets;
        private int receivedPackets;
        private Thread worker;

        public Receiver(IPv4   ipAddress,
                        ushort localPort,
                        ushort remotePort,
                        int    maxReceivePackets)
        {
            Core c = Core.Instance();
            UdpModule m  = c.GetProtocolByName("UDP") as UdpModule;
            this.session = m.CreateBoundSession(ipAddress, localPort,
                                                ipAddress, remotePort);

            this.session.Blocking  = true;
            this.maxReceivePackets = maxReceivePackets;
            this.receivedPackets   = 0;

            this.worker = new Thread(new ThreadStart(WorkLoop));
            this.worker.Start();
        }

        public ThreadState ThreadState { get { return worker.ThreadState; } }

        private void WorkLoop()
        {
            while (receivedPackets++ != maxReceivePackets) {
                byte[] recvData = session.ReadData();
                System.Diagnostics.Debug.Assert(recvData.Length == 4);
                //
                // Get Sequence number from payload and assert it is
                // the expected one.
                //
                int v = BitConverter.ToInt32(recvData, 0);
                v = ByteOrder.BigEndianToHost(v);
                System.Diagnostics.Debug.Assert(v == receivedPackets);

                if ((receivedPackets % 100) == 0)
                    Console.WriteLine("Received {0}", receivedPackets);
            }
        }
    }

    public class Sender
    {
        private UdpSession session;

        private int maxSendPackets;
        private int sentPackets;
        private Thread worker;

        public Sender(IPv4   ipAddress,
                      ushort localPort,
                      ushort remotePort,
                      int    maxSendPackets)
        {
            Core c = Core.Instance();
            UdpModule m  = c.GetProtocolByName("UDP") as UdpModule;
            this.session = m.CreateBoundSession(ipAddress, localPort,
                                                ipAddress, remotePort);
            this.session.Blocking = true;

            this.maxSendPackets = maxSendPackets;
            this.sentPackets    = 0;

            this.worker = new Thread(new ThreadStart(WorkLoop));
            this.worker.Start();
        }

        public ThreadState ThreadState { get { return worker.ThreadState; } }

        private void WorkLoop()
        {
            while (sentPackets++ != maxSendPackets) {
                int n = ByteOrder.HostToBigEndian(sentPackets);
                int r = session.WriteData(BitConverter.GetBytes(n));
                //                Console.WriteLine("Send {0}", sentPackets);
            }
        }
    }

    public class Test
    {
        static void Main()
        {
            StaticConfiguration.Initialize();
            StaticConfiguration.Start();

            LoopbackAdapter loopback = new LoopbackAdapter();
            Console.WriteLine("Created Loopback Adapter {0}",
                              loopback.HardwareAddress);
            Core.Instance().RegisterAdapter(loopback, 64);

            IPModule ip = Core.Instance().GetProtocolByName("IP") as
                IPModule;

            Console.WriteLine("Binding address to adapter");

            IPv4 ifAddress = IPv4.Parse("192.168.0.100");
            IPv4 ifNetmask = IPv4.Parse("255.255.255.0");
            IPv4 gwAddress = IPv4.Parse("192.168.0.1");

            ip.HostConfiguration.Bindings.Add(
                loopback,
                new InterfaceIPConfiguration(ifAddress, ifNetmask, gwAddress)
                );


            const int nInstances = 8;
            const int maxPackets = 10000;
            ushort    basePort   = 10000;

            Receiver[] receivers = new Receiver[nInstances];
            Sender[]   senders   = new Sender[nInstances];

            for (int i = 0; i < nInstances; i++) {
                ushort rPort = (ushort)(basePort + 2 * i);
                ushort sPort = (ushort)(basePort + 2 * i + 1);
                receivers[i] =
                    new Receiver(ifAddress, rPort, sPort, maxPackets);
                senders[i] = new Sender(ifAddress, sPort, rPort, maxPackets);
            }

            bool done = false;
            while (done == false) {
                Thread.CurrentThread.Join(TimeSpan.FromSeconds(1));
                done = true;
                for (int i = 0; i < nInstances; i++) {
                    if (receivers[i].ThreadState != ThreadState.Stopped ||
                        senders[i].ThreadState   != ThreadState.Stopped)
                    {
                        done = false;
                        break;
                    }
                }
            }

            Console.WriteLine("Removing Adapter.");
            Core.Instance().DeregisterAdapter(loopback);

            StaticConfiguration.Stop();
        }
    }
}
