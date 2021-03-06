﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Tests.Diagnostics
{
    public class MyAttribute : Attribute { }

    class UnusedPrivateMember
    {
        public static void Main() { }

        private class MyOtherClass
        { }

        private class MyClass
        {
            internal MyClass(int i)
            {
                var x = (MyOtherClass)null;
                x = x as MyOtherClass;
                Console.WriteLine();
            }
        }

        private class Gen<T> : MyClass
        {
            public Gen() : base(1)
            {
                Console.WriteLine();
            }

            public Gen(int i) : this() // Noncompliant {{Remove the unused private constructor 'Gen'.}}
            {
                Console.WriteLine();
            }
        }

        public UnusedPrivateMember()
        {
            MyProperty = 5;
            MyEvent += UnusedPrivateMember_MyEvent;
            new Gen<int>();
        }

        private void UnusedPrivateMember_MyEvent()
        {
            field3 = 5;
            throw new NotImplementedException();
        }

        private int field, field2; // Noncompliant
//      ^^^^^^^^^^^^^^^^^^^^^^^^^^
        private
            int field3, field4; // Noncompliant;
//                      ^^^^^^
        private int Property // Noncompliant {{Remove the unused private property 'Property'.}}
        {
            get; set;
        }
        private void Method() { } // Noncompliant {{Remove the unused private method 'Method'.}}
        private class Class { }// Noncompliant {{Remove the unused private type 'Class'.}}
//      ^^^^^^^^^^^^^^^^^^^^^^^
        private struct Struct { }// Noncompliant {{Remove the unused private type 'Struct'.}}
//      ^^^^^^^^^^^^^^^^^^^^^^^^^
        private delegate void Delegate();
        private delegate void Delegate2(); // Noncompliant {{Remove the unused private type 'Delegate2'.}}
        private event Delegate Event; //Noncompliant {{Remove the unused private event 'Event'.}}
        private event Delegate MyEvent;

        private int MyProperty
        {
            get;
            set;
        }

        [My]
        private class Class1 { }

        private class Class2
        {
            private Class2() // Compliant
            {
            }
            private Class2(int i) // Noncompliant {{Remove the unused private constructor 'Class2'.}}
            {
                new Class2("").field2 = 3;
            }
            private Class2(string i)
            {
            }
            public int field; // Noncompliant {{Remove the unused private field 'field'.}}
            public int field2;
        }

        private interface MyInterface
        {
            void Method();
        }
        private class Class3 : MyInterface // Noncompliant {{Remove the unused private type 'Class3'.}}
        {
            public void Method() { var x = this[20]; }
            public void Method1() { var x = Method2(); } // Noncompliant {{Remove the unused private method 'Method1'.}}
            public static int Method2() { return 2; }

            public int this[int index]
            {
                get { return 42; }
            }
        }
    }
    public static class MyExtension
    {
        private static void MyMethod<T>(this T self) { "".MyMethod<string>(); }
    }

    public class NonExactMatch
    {
        private static void M(int i) { }    // Compliant, might be called
        private static void M(string i) { } // Compliant, might be called

        public static void Call(dynamic d)
        {
            M(d);
        }
    }

    public class EventHandlerSample
    {
        private void MyOnClick(object sender, EventArgs args) { }
    }

    public class PropertyAccess
    {
        private int OnlyRead { get; set; }  // Noncompliant {{Remove the unused private set accessor in property 'OnlyRead'.}}
//                                  ^^^^
        private int OnlySet { get; set; }
        private int OnlySet2 { get { return 42; } set { } } // Noncompliant {{Remove the unused private get accessor in property 'OnlySet2'.}}
//                             ^^^^^^^^^^^^^^^^^^
        private int NotAccessed { get; set; }   // Noncompliant {{Remove the unused private property 'NotAccessed'.}}
//      ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        private int BothAccessed { get; set; }

        private int OnlyGet { get { return 42; } }

        public void M()
        {
            Console.WriteLine(OnlyRead);
            OnlySet = 42;
            (this.OnlySet2) = 42;

            BothAccessed++;

            int? x = 10;
            x = this?.OnlyGet;
        }
    }

    [Serializable]
    public sealed class GoodException : Exception
    {
        public GoodException()
        {
        }
        public GoodException(string message)
            : base(message)
        {
        }
        public GoodException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        private GoodException(SerializationInfo info, StreamingContext context) // Compliant because of the serialization
            : base(info, context)
        {
        }
    }
}