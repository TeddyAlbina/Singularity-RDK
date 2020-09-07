// ==++==
//
//   Copyright (c) Microsoft Corporation.  All rights reserved.
//
// ==--==
#define MINRUNTIME

namespace System
{
    using Microsoft.Singularity;
#if !MINRUNTIME
    using Microsoft.Singularity.Io;
#endif
    using System;
    using System.Text;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    using System.Runtime.InteropServices;

    // Provides static fields for console input and output.  Use
    // Console.In for input from the standard input stream (stdin),
    // Console.Out for output to stdout, and Console.Error
    // for output to stderr.  If any of those console streams are
    // redirected from the command line, these streams will be redirected.
    // A program can also redirect its own output or input with the
    // SetIn, SetOut, and SetError methods.
    //
    // The distinction between Console.Out and Console.Error is useful
    // for programs that redirect output to a file or a pipe.  Note that
    // stdout and stderr can be output to different files at the same
    // time from the DOS command line:
    //
    // someProgram 1> out 2> err
    //
    //| <include path='docs/doc[@for="Console"]/*' />
    public sealed class Console
    {
        private Console()
        {
            throw new NotSupportedException("NotSupported_Constructor");
        }

        static Console() {
        }


        private static void WriteInternal(char[] buffer, int index, int count)
        {
#if !MINRUNTIME
            ConsoleOutput.Write(buffer, index, count);
#else
            for (int i = 0; i < count; i++) {
                DebugStub.Print(buffer[index + i]);
            }
#endif
        }

        private static void WriteInternal(String value)
        {
#if !MINRUNTIME
            ConsoleOutput.Write(value);
#else
            DebugStub.Print(value);
#endif
        }

        private static void WriteNewLine()
        {
            WriteInternal(Environment.NewLine);
        }

        //| <include path='docs/doc[@for="Console.WriteLine"]/*' />
        public static void WriteLine()
        {
            WriteNewLine();
        }

        //| <include path='docs/doc[@for="Console.WriteLine13"]/*' />
        public static void WriteLine(String value)
        {
            WriteInternal(value);
            WriteNewLine();
        }

        //| <include path='docs/doc[@for="Console.WriteLine1"]/*' />
        public static void WriteLine(bool value)
        {
            WriteInternal(value.ToString());
            WriteNewLine();
        }

        //| <include path='docs/doc[@for="Console.WriteLine2"]/*' />
        public static void WriteLine(char value)
        {
            WriteInternal(value.ToString());
            WriteNewLine();
        }

        //| <include path='docs/doc[@for="Console.WriteLine3"]/*' />
        public static void WriteLine(char[] buffer)
        {
            WriteInternal(buffer, 0, buffer.Length);
            WriteNewLine();
        }

        //| <include path='docs/doc[@for="Console.WriteLine4"]/*' />
        public static void WriteLine(char[] buffer, int index, int count)
        {
            WriteInternal(buffer, index, count);
            WriteNewLine();
        }

        //| <include path='docs/doc[@for="Console.WriteLine5"]/*' />
        public static void WriteLine(decimal value)
        {
            WriteInternal(value.ToString());
            WriteNewLine();
        }

        //| <include path='docs/doc[@for="Console.WriteLine6"]/*' />
        public static void WriteLine(double value)
        {
            WriteInternal(value.ToString());
            WriteNewLine();
        }

        //| <include path='docs/doc[@for="Console.WriteLine7"]/*' />
        public static void WriteLine(float value)
        {
            WriteInternal(value.ToString());
            WriteNewLine();
        }

        //| <include path='docs/doc[@for="Console.WriteLine8"]/*' />
        public static void WriteLine(int value)
        {
            WriteInternal(value.ToString());
            WriteNewLine();
        }

        //| <include path='docs/doc[@for="Console.WriteLine9"]/*' />
        [CLSCompliant(false)]
        public static void WriteLine(uint value)
        {
            WriteInternal(value.ToString());
            WriteNewLine();
        }

        //| <include path='docs/doc[@for="Console.WriteLine10"]/*' />
        public static void WriteLine(long value)
        {
            WriteInternal(value.ToString());
            WriteNewLine();
        }

        //| <include path='docs/doc[@for="Console.WriteLine11"]/*' />
        [CLSCompliant(false)]
        public static void WriteLine(ulong value)
        {
            WriteInternal(value.ToString());
            WriteNewLine();
        }

        //| <include path='docs/doc[@for="Console.WriteLine12"]/*' />
        public static void WriteLine(Object value)
        {
            WriteInternal(value.ToString());
            WriteNewLine();
        }

        //| <include path='docs/doc[@for="Console.WriteLine14"]/*' />
        public static void WriteLine(String format, Object arg0)
        {
            WriteInternal(String.Format(format, arg0));
            WriteNewLine();
        }

        //| <include path='docs/doc[@for="Console.WriteLine15"]/*' />
        public static void WriteLine(String format, Object arg0, Object arg1)
        {
            WriteInternal(String.Format(format, arg0, arg1));
            WriteNewLine();
        }

        //| <include path='docs/doc[@for="Console.WriteLine16"]/*' />
        public static void WriteLine(String format, Object arg0, Object arg1, Object arg2)
        {
            WriteInternal(String.Format(format, arg0, arg1, arg2));
            WriteNewLine();
        }

        //| <include path='docs/doc[@for="Console.WriteLine18"]/*' />
        public static void WriteLine(String format, params Object[] args)
        {
            WriteInternal(String.Format(format, args));
            WriteNewLine();
        }

        //| <include path='docs/doc[@for="Console.Write"]/*' />
        public static void Write(String format, Object arg0)
        {
            WriteInternal(String.Format(format, arg0));
        }

        //| <include path='docs/doc[@for="Console.Write1"]/*' />
        public static void Write(String format, Object arg0, Object arg1)
        {
            WriteInternal(String.Format(format, arg0, arg1));
        }

        //| <include path='docs/doc[@for="Console.Write2"]/*' />
        public static void Write(String format, Object arg0, Object arg1, Object arg2)
        {
            WriteInternal(String.Format(format, arg0, arg1, arg2));
        }

        //| <include path='docs/doc[@for="Console.Write4"]/*' />
        public static void Write(String format, params Object[] args)
        {
            WriteInternal(String.Format(format, args));
        }

        //| <include path='docs/doc[@for="Console.Write5"]/*' />
        public static void Write(bool value)
        {
            WriteInternal(value.ToString());
        }

        //| <include path='docs/doc[@for="Console.Write6"]/*' />
        public static void Write(char value)
        {
            WriteInternal(value.ToString());
        }

        //| <include path='docs/doc[@for="Console.Write7"]/*' />
        public static void Write(char[] buffer)
        {
            WriteInternal(buffer, 0, buffer.Length);
        }

        //| <include path='docs/doc[@for="Console.Write8"]/*' />
        public static void Write(char[] buffer, int index, int count)
        {
            WriteInternal(buffer, index, count);
        }

        //| <include path='docs/doc[@for="Console.Write9"]/*' />
        public static void Write(double value)
        {
            WriteInternal(value.ToString());
        }

        //| <include path='docs/doc[@for="Console.Write10"]/*' />
        public static void Write(decimal value)
        {
            WriteInternal(value.ToString());
        }

        //| <include path='docs/doc[@for="Console.Write11"]/*' />
        public static void Write(float value)
        {
            WriteInternal(value.ToString());
        }

        //| <include path='docs/doc[@for="Console.Write12"]/*' />
        public static void Write(int value)
        {
            WriteInternal(value.ToString());
        }

        //| <include path='docs/doc[@for="Console.Write13"]/*' />
        [CLSCompliant(false)]
        public static void Write(uint value)
        {
            WriteInternal(value.ToString());
        }

        //| <include path='docs/doc[@for="Console.Write14"]/*' />
        public static void Write(long value)
        {
            WriteInternal(value.ToString());
        }

        //| <include path='docs/doc[@for="Console.Write15"]/*' />
        [CLSCompliant(false)]
        public static void Write(ulong value)
        {
            WriteInternal(value.ToString());
        }

        //| <include path='docs/doc[@for="Console.Write16"]/*' />
        public static void Write(Object value)
        {
            WriteInternal(value.ToString());
        }

        //| <include path='docs/doc[@for="Console.Write17"]/*' />
        public static void Write(String value)
        {
            WriteInternal(value);
        }

        public static int Read()
        {
#if !MINRUNTIME
            return ConsoleInput.ReadChar(); 
#else
            return -1;
#endif
        }
        public static string ReadLine()
        {
#if !MINRUNTIME
            return ConsoleInput.ReadLine();
#else
            return "";
#endif
        }

        public static System.IO.TextWriter Out { get { return new ConsoleWriter(); } }
        public static System.IO.TextWriter Error { get { return new ConsoleWriter(); } }
        public static System.IO.Stream OpenStandardOutput() {return null;}
        public static System.IO.Stream OpenStandardInput() {return null;}
        public static System.IO.Stream OpenStandardInput(int bufferSize) {return null;}
    }

    internal class ConsoleWriter: System.IO.TextWriter
    {
        public override System.Text.Encoding Encoding {
            get { return new System.Text.ASCIIEncoding(); }
        }

        public override void Write(char value) {
            Console.Write(value);
        }
    }
}
