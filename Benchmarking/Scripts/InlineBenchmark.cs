using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Mirror.Benchmark.InlineCode
{
    public class InlineBenchmark : MonoBehaviour
    {
        Stopwatch sw = Stopwatch.StartNew();


        WriteToPrivateField privateField;
        WriteToPublicField publicField;
        IWrite privateFieldInterface;
        IWrite publicFieldInterface;
        IWriteInlining privateFieldInterfaceInline;
        IWriteInlining publicFieldInterfaceInline;

        const int arrayLength = 10_000;
        private void Start()
        {
            try
            {
                const int iterations = 1_000_000_000;


                privateField = new WriteToPrivateField(arrayLength);
                publicField = new WriteToPublicField(arrayLength);
                privateFieldInterface = new WriteToPrivateFieldWithInterface(arrayLength);
                publicFieldInterface = new WriteToPublicFieldWithInterface(arrayLength);
                privateFieldInterfaceInline = new WriteToPrivateFieldWithInterfaceInlining(arrayLength);
                publicFieldInterfaceInline = new WriteToPublicFieldWithInterfaceInlining(arrayLength);

                Console.WriteLine($"\n-------------\nRotation Only\n\n");
                testOne(nameof(private_noline), private_noline, iterations);
                testOne(nameof(private_aggressive), private_aggressive, iterations);
                testOne(nameof(public_noline), public_noline, iterations);
                testOne(nameof(public_aggressive), public_aggressive, iterations);
                testOne(nameof(public_manual), public_manual, iterations);
                testOne(nameof(private_interface_noline), private_interface_noline, iterations);
                testOne(nameof(private_interface_aggressive), private_interface_aggressive, iterations);
                testOne(nameof(public_interface_noline), public_interface_noline, iterations);
                testOne(nameof(public_interface_aggressive), public_interface_aggressive, iterations);
                testOne(nameof(private_interfaceinline_noline), private_interfaceinline_noline, iterations);
                testOne(nameof(private_interfaceinline_aggressive), private_interfaceinline_aggressive, iterations);
                testOne(nameof(public_interfaceinline_noline), public_interfaceinline_noline, iterations);
                testOne(nameof(public_interfaceinline_aggressive), public_interfaceinline_aggressive, iterations);
                Console.WriteLine($"\n\n\n-------------\n\n");
            }
            finally
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
        }

        void testOne(string name, Action<int> action, int iterations)
        {
            // warmup (atleast 10)
            action.Invoke(Mathf.Max(iterations / 100, 10));
            long start = sw.ElapsedMilliseconds;
            action.Invoke(iterations);

            long end = sw.ElapsedMilliseconds;
            long elapsed = end - start;
            Console.WriteLine($"{name,-50}{elapsed,10}");
        }


        void private_noline(int count)
        {
            for (int i = 0; i < count; i++)
            {
                privateField.WriteNoLine((byte)i);
            }
        }
        void private_aggressive(int count)
        {
            for (int i = 0; i < count; i++)
            {
                privateField.WriteAggressive((byte)i);
            }
        }

        void public_noline(int count)
        {
            for (int i = 0; i < count; i++)
            {
                publicField.WriteNoLine((byte)i);
            }
        }
        void public_aggressive(int count)
        {
            for (int i = 0; i < count; i++)
            {
                publicField.WriteAggressive((byte)i);
            }
        }
        void public_manual(int count)
        {
            for (int i = 0; i < count; i++)
            {
                publicField.buffer[publicField.next] = (byte)i;
                publicField.next++;
                if (publicField.next >= publicField.buffer.Length) { publicField.next = 0; }
            }
        }

        void private_interface_noline(int count)
        {
            for (int i = 0; i < count; i++)
            {
                privateFieldInterface.WriteNoLine((byte)i);
            }
        }
        void private_interface_aggressive(int count)
        {
            for (int i = 0; i < count; i++)
            {
                privateFieldInterface.WriteAggressive((byte)i);
            }
        }

        void public_interface_noline(int count)
        {
            for (int i = 0; i < count; i++)
            {
                publicFieldInterface.WriteNoLine((byte)i);
            }
        }
        void public_interface_aggressive(int count)
        {
            for (int i = 0; i < count; i++)
            {
                publicFieldInterface.WriteAggressive((byte)i);
            }
        }

        void private_interfaceinline_noline(int count)
        {
            for (int i = 0; i < count; i++)
            {
                privateFieldInterfaceInline.WriteNoLine((byte)i);
            }
        }
        void private_interfaceinline_aggressive(int count)
        {
            for (int i = 0; i < count; i++)
            {
                privateFieldInterfaceInline.WriteAggressive((byte)i);
            }
        }

        void public_interfaceinline_noline(int count)
        {
            for (int i = 0; i < count; i++)
            {
                publicFieldInterfaceInline.WriteNoLine((byte)i);
            }
        }
        void public_interfaceinline_aggressive(int count)
        {
            for (int i = 0; i < count; i++)
            {
                publicFieldInterfaceInline.WriteAggressive((byte)i);
            }
        }
    }

    public class WriteToPrivateField
    {
        byte[] buffer;
        int next;

        public WriteToPrivateField(int length)
        {
            buffer = new byte[length];
        }

        public void WriteNoLine(byte a)
        {
            buffer[next] = a;
            next++;
            if (next >= buffer.Length) { next = 0; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteAggressive(byte a)
        {
            buffer[next] = a;
            next++;
            if (next >= buffer.Length) { next = 0; }
        }

    }

    public class WriteToPublicField
    {
        public byte[] buffer;
        public int next;
        public WriteToPublicField(int length)
        {
            buffer = new byte[length];
        }
        public void WriteNoLine(byte a)
        {
            buffer[next] = a;
            next++;
            if (next >= buffer.Length) { next = 0; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteAggressive(byte a)
        {
            buffer[next] = a;
            next++;
            if (next >= buffer.Length) { next = 0; }
        }
    }

    public interface IWrite
    {
        void WriteNoLine(byte a);
        void WriteAggressive(byte a);
    }

    public class WriteToPrivateFieldWithInterface : IWrite
    {
        byte[] buffer;
        int next;
        public WriteToPrivateFieldWithInterface(int length)
        {
            buffer = new byte[length];
        }
        public void WriteNoLine(byte a)
        {
            buffer[next] = a;
            next++;
            if (next >= buffer.Length) { next = 0; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteAggressive(byte a)
        {
            buffer[next] = a;
            next++;
            if (next >= buffer.Length) { next = 0; }
        }
    }

    public class WriteToPublicFieldWithInterface : IWrite
    {
        public byte[] buffer;
        public int next;
        public WriteToPublicFieldWithInterface(int length)
        {
            buffer = new byte[length];
        }
        public void WriteNoLine(byte a)
        {
            buffer[next] = a;
            next++;
            if (next >= buffer.Length) { next = 0; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteAggressive(byte a)
        {
            buffer[next] = a;
            next++;
            if (next >= buffer.Length) { next = 0; }
        }
    }


    public interface IWriteInlining
    {
        void WriteNoLine(byte a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void WriteAggressive(byte a);
    }

    public class WriteToPrivateFieldWithInterfaceInlining : IWriteInlining
    {
        byte[] buffer;
        int next;
        public WriteToPrivateFieldWithInterfaceInlining(int length)
        {
            buffer = new byte[length];
        }
        public void WriteNoLine(byte a)
        {
            buffer[next] = a;
            next++;
            if (next >= buffer.Length) { next = 0; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteAggressive(byte a)
        {
            buffer[next] = a;
            next++;
            if (next >= buffer.Length) { next = 0; }
        }
    }

    public class WriteToPublicFieldWithInterfaceInlining : IWriteInlining
    {
        public byte[] buffer;
        public int next;
        public WriteToPublicFieldWithInterfaceInlining(int length)
        {
            buffer = new byte[length];
        }
        public void WriteNoLine(byte a)
        {
            buffer[next] = a;
            next++;
            if (next >= buffer.Length) { next = 0; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteAggressive(byte a)
        {
            buffer[next] = a;
            next++;
            if (next >= buffer.Length) { next = 0; }
        }
    }
}
