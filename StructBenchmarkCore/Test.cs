﻿using System;
using System.Diagnostics;

namespace StructBenchmarkCore
{
    public struct PointStruct
    {
        public PointStruct( long x, long y )
        {
            X = x;
            Y = y;
        }

        public readonly long X, Y;
    }

    public class PointClass
    {
        public PointClass( long x, long y )
        {
            X = x;
            Y = y;
        }

        public readonly long X, Y;
    }

    public struct Point3Struct
    {
        public Point3Struct( long x, long y, long z )
        {
            X = x;
            Y = y;
            Z = z;
        }

        public readonly long X, Y, Z;
    }

    public class Point3Class
    {
        public Point3Class( long x, long y, long z )
        {
            X = x;
            Y = y;
            Z = z;
        }

        public readonly long X, Y, Z;
    }

    public struct Point4Struct
    {
        public Point4Struct( long x, long y, long z, long a )
        {
            X = x;
            Y = y;
            Z = z;
            A = a;
        }

        public readonly long X, Y, Z, A;
    }

    public class Point4Class
    {
        public Point4Class( long x, long y, long z, long a )
        {
            X = x;
            Y = y;
            Z = z;
            A = a;
        }

        public readonly long X, Y, Z, A;
    }

    public struct Point8Struct
    {
        public Point8Struct( long x, long y, long z, long a, long b, long c, long d, long e )
        {
            X = x;
            Y = y;
            Z = z;
            A = a;
            B = b;
            C = c;
            D = d;
            E = e;
        }

        public readonly long X, Y, Z, A, B, C, D, E;
    }

    public struct Struct2Ref
    {
        public readonly long Z;
        public readonly PointClass XY;

        public Struct2Ref( long x, long y, long z )
        {
            Z = z;
            XY = new PointClass( x, y );
        }
    }

    public class Class2Ref
    {
        public readonly long Z;
        public readonly PointClass XY;

        public Class2Ref( long x, long y, long z )
        {
            Z = z;
            XY = new PointClass( x, y );
        }
    }

	public struct PointStructWithRefs
    {
        public PointStructWithRefs( PointClass x, PointClass y )
        {
            X = x;
            Y = y;
        }

        public readonly PointClass X, Y;
    }

	public struct PointStructWith3Refs
    {
        public PointStructWith3Refs( PointClass x, PointClass y, PointClass z )
        {
            X = x;
            Y = y;
            Z = z;
        }

        public readonly PointClass X, Y, Z;
    }

	public class PointClassWithRefs
    {
        public PointClassWithRefs( PointClass x, PointClass y )
        {
            X = x;
            Y = y;
        }

        public readonly PointClass X, Y;
    }

    public class GCStats
    {
        private long _memoryBefore;
        private long _memoryAfter;
        private long[] _gcCount = new long[ 3 ];

        public void StartGathering()
        {
            _memoryBefore = GC.GetTotalMemory( true );
            for( var i = 0; i <= GC.MaxGeneration; i++ )
            {
                this._gcCount[ i ] = GC.CollectionCount( i );
            }
        }

        public void StopGathering()
        {
            this._memoryAfter = GC.GetTotalMemory(true);
            for ( var i = 0; i <= GC.MaxGeneration; i++ )
            {
                this._gcCount[ i ] = GC.CollectionCount( i ) - this._gcCount[ i ];
            }
            this._memoryAfter = GC.GetTotalMemory( true );
        }

        public void PrintStats()
        {
            Console.WriteLine( "Mem before: {0}KB; Mem after: {1}KB; GC 0 - {2}; GC 1 - {3}; GC 2 - {4}", this._memoryBefore / 1024, this._memoryAfter / 1024, this._gcCount[ 0 ], this._gcCount[ 1 ], this._gcCount[ 2 ] );
        }
    }

    public static class CSharpTest
    {
        private const int iterations = 10 * 1000 * 1000;
        private const int copyCount = 10;
        private static long _accumulator;

        private static void ReportMemory()
        {
            Console.WriteLine("Memory Consumed - {0}MB", GC.GetTotalMemory(false) / 1024d / 1024d);
        }

        public static void Measure( string name, Action act )
        {
            var gcStats = new GCStats();
            gcStats.StartGathering();

            var sw = Stopwatch.StartNew();
            act();
            sw.Stop();
            Console.WriteLine( "Accumulator - " + _accumulator );
            Console.WriteLine( "Measured '{0}' - {1}ms", name, sw.ElapsedMilliseconds );

            gcStats.StopGathering();
            gcStats.PrintStats();
            Console.WriteLine();
        }

        private static void TestStruct()
        {
            var data = new PointStruct[ iterations ];

            for( var i = 0; i < iterations; i++ )
            {
                data[ i ] = new PointStruct( i, i + 1 );
            }

            long accumulator = 0;
            for( var i = 0; i < iterations; i++ )
            {
                var d = data[ i ];
                accumulator += d.X + d.Y;
            }
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X + d.Y;
            }
            ReportMemory();
            _accumulator = accumulator;
        }


        private static void TestClass()
        {
            var data = new PointClass[ iterations ];

            for( var i = 0; i < iterations; i++ )
            {
                data[ i ] = new PointClass( i, i + 1 );
            }

            long accumulator = 0;
            for( var i = 0; i < iterations; i++ )
            {
                var d = data[ i ];
                accumulator += d.X + d.Y;
            }
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X + d.Y;
            }
            ReportMemory();
            _accumulator = accumulator;
        }

        private static void Test3Struct()
        {
            var data = new Point3Struct[ iterations ];

            for( var i = 0; i < iterations; i++ )
            {
                data[ i ] = new Point3Struct( i, i + 1, i + 2 );
            }

            long accumulator = 0;
            for( var i = 0; i < iterations; i++ )
            {
                var d = data[ i ];
                accumulator += d.X + d.Y + d.Z;
            }
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X + d.Y + d.Z;
            }
            ReportMemory();
            _accumulator = accumulator;
        }

        private static void Test3Class()
        {
            var data = new Point3Class[ iterations ];

            for( var i = 0; i < iterations; i++ )
            {
                data[ i ] = new Point3Class( i, i + 1, i + 2 );
            }

            long accumulator = 0;
            for( var i = 0; i < iterations; i++ )
            {
                var d = data[ i ];
                accumulator += d.X + d.Y + d.Z;
            }
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X + d.Y + d.Z;
            }
            ReportMemory();
            _accumulator = accumulator;
        }

        private static void Test4Struct()
        {
            var data = new Point4Struct[ iterations ];

            for( var i = 0; i < iterations; i++ )
            {
                data[ i ] = new Point4Struct( i, i + 1, i + 2, i + 3 );
            }

            long accumulator = 0;
            for( var i = 0; i < iterations; i++ )
            {
                var d = data[ i ];
                accumulator += d.X + d.Y + d.Z + d.A;
            }
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X + d.Y + d.Z + d.A;
            }
            ReportMemory();
            _accumulator = accumulator;
        }

        private static void Test4Class()
        {
            var data = new Point4Class[ iterations ];

            for( var i = 0; i < iterations; i++ )
            {
                data[ i ] = new Point4Class( i, i + 1, i + 2, i + 3 );
            }

            long accumulator = 0;
            for( var i = 0; i < iterations; i++ )
            {
                var d = data[ i ];
                accumulator += d.X + d.Y + d.Z + d.A;
            }
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X + d.Y + d.Z + d.A;
            }
            ReportMemory();
            _accumulator = accumulator;
        }

        private static void Test8Struct()
        {
            var data = new Point8Struct[ iterations ];

            for( var i = 0; i < iterations; i++ )
            {
                data[ i ] = new Point8Struct( i, i + 1, i + 2, i + 3, i + 4, i + 5, i + 6, i + 7 );
            }

            long accumulator = 0;
            for( var i = 0; i < iterations; i++ )
            {
                var d = data[ i ];
                accumulator += d.X + d.Y + d.Z + d.A + d.B + d.C + d.D + d.E;
            }
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X + d.Y + d.Z + d.A + d.B + d.C + d.D + d.E;
            }
            ReportMemory();
            _accumulator = accumulator;
        }

        private static void Test3StructRef()
        {
            var data = new Struct2Ref[ iterations ];

            for( var i = 0; i < iterations; i++ )
            {
                data[ i ] = new Struct2Ref( i, i + 1, i + 2 );
            }

            long accumulator = 0;
            for( var i = 0; i < iterations; i++ )
            {
                var d = data[ i ];
                accumulator += d.XY.X + d.XY.Y + d.Z;
            }
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.XY.X + d.XY.Y + d.Z;
            }
            ReportMemory();
            _accumulator = accumulator;
        }

        private static void Test3ClassRef()
        {
            var data = new Class2Ref[ iterations ];

            for( var i = 0; i < iterations; i++ )
            {
                data[ i ] = new Class2Ref( i, i + 1, i + 2 );
            }

            long accumulator = 0;
            for( var i = 0; i < iterations; i++ )
            {
                var d = data[ i ];
                accumulator += d.XY.X + d.XY.Y + d.Z;
            }
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.XY.X + d.XY.Y + d.Z;
            }
            ReportMemory();
            _accumulator = accumulator;
        }

        private static void CopyStruct2Test()
        {
            var di = new PointStruct[iterations];
            var data = new PointStruct[iterations];

            for (var i = 0; i < iterations; i++)
            {
                di[i] = new PointStruct(i, i + 1);
            }

            for (var z = 0; z < copyCount; z++)
            {
                for (var i = 0; i < iterations; i++)
                {
                    data[i] = di[i];
                }
            }

            long accumulator = 0;
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X + d.Y;
            }
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X + d.Y;
            }
            ReportMemory();
            _accumulator = accumulator;
        }

        private static void TestStructWithRefs()
        {
            var data = new PointStructWithRefs[ iterations ];

            for( var i = 0; i < iterations; i++ )
            {
				var x = new PointClass(i, i+1);
				var y = new PointClass(i, i+1);
                data[ i ] = new PointStructWithRefs( x, y );
            }

            long accumulator = 0;
            for( var i = 0; i < iterations; i++ )
            {
                var d = data[ i ];
                accumulator += d.X.X + d.Y.Y;
            }
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X.X + d.Y.Y;
            }
            ReportMemory();
            _accumulator = accumulator;
        }

        private static void TestStructWith3Refs()
        {
            var data = new PointStructWith3Refs[ iterations ];

            for( var i = 0; i < iterations; i++ )
            {
				var x = new PointClass(i, i+1);
				var y = new PointClass(i, i+1);
				var z = new PointClass(i, i+1);
                data[ i ] = new PointStructWith3Refs( x, y, z );
            }

            long accumulator = 0;
            for( var i = 0; i < iterations; i++ )
            {
                var d = data[ i ];
                accumulator += d.X.X + d.Y.Y + d.Z.X;
            }
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X.X + d.Y.Y + d.Z.X;
            }
            ReportMemory();
            _accumulator = accumulator;
        }
        private static void TestClassWithRefs()
        {
            var data = new PointClassWithRefs[ iterations ];

            for( var i = 0; i < iterations; i++ )
            {
				var x = new PointClass(i, i+1);
				var y = new PointClass(i, i+1);
                data[ i ] = new PointClassWithRefs( x, y );
            }

            long accumulator = 0;
            for( var i = 0; i < iterations; i++ )
            {
                var d = data[ i ];
                accumulator += d.X.X + d.Y.Y;
            }
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X.X + d.Y.Y;
            }
            ReportMemory();
            _accumulator = accumulator;
        }

        private static void CopyStructWithLinksTest()
        {
            var di = new PointStructWithRefs[iterations];
            var data = new PointStructWithRefs[iterations];

            for (var i = 0; i < iterations; i++)
            {
                di[i] = new PointStructWithRefs(new PointClass(i, i+1), new PointClass(i, i+1));
            }

            for (var z = 0; z < copyCount; z++)
            {
                for (var i = 0; i < iterations; i++)
                {
                    data[i] = di[i];
                }
            }

            long accumulator = 0;
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X.X + d.Y.Y;
            }
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X.X + d.Y.Y;
            }
            ReportMemory();
            _accumulator = accumulator;
        }

        private static void CopyStructWith3LinksTest()
        {
            var di = new PointStructWith3Refs[iterations];
            var data = new PointStructWith3Refs[iterations];

            for (var i = 0; i < iterations; i++)
            {
                di[i] = new PointStructWith3Refs(
	                new PointClass(i, i+1),
	                new PointClass(i, i+1),
	                new PointClass(i, i+1));
            }

            for (var z = 0; z < copyCount; z++)
            {
                for (var i = 0; i < iterations; i++)
                {
                    data[i] = di[i];
                }
            }

            long accumulator = 0;
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X.X + d.Y.Y + d.Z.X;
            }
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X.X + d.Y.Y + d.Z.X;
            }
            ReportMemory();
            _accumulator = accumulator;
        }

        private static void CopyStruct3Test()
        {
            var di = new Point3Struct[iterations];
            var data = new Point3Struct[iterations];

            for (var i = 0; i < iterations; i++)
            {
                di[i] = new Point3Struct(i, i + 1, i + 2);
            }

            for (var z = 0; z < copyCount; z++)
            {
                for (var i = 0; i < iterations; i++)
                {
                    data[i] = di[i];
                }
            }

            long accumulator = 0;
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X + d.Y + d.Z;
            }
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X + d.Y + d.Z;
            }
            ReportMemory();
            _accumulator = accumulator;
        }

        private static void CopyStruct4Test()
        {
            var di = new Point4Struct[iterations];
            var data = new Point4Struct[iterations];

            for (var i = 0; i < iterations; i++)
            {
                di[i] = new Point4Struct(i, i + 1, i + 2, i + 3);
            }

            for (var z = 0; z < copyCount; z++)
            {
                for (var i = 0; i < iterations; i++)
                {
                    data[i] = di[i];
                }
            }

            long accumulator = 0;
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X + d.Y + d.Z + d.A;
            }
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X + d.Y + d.Z + d.A;
            }
            ReportMemory();
            _accumulator = accumulator;
        }

        private static void CopyStruct8Test()
        {
            var di = new Point8Struct[iterations];
            var data = new Point8Struct[iterations];

            for (var i = 0; i < iterations; i++)
            {
                di[i] = new Point8Struct(i, i + 1, i + 2, i + 3, i + 4, i + 5, i + 6, i + 7);
            }

            for (var z = 0; z < copyCount; z++)
            {
                for (var i = 0; i < iterations; i++)
                {
                    data[i] = di[i];
                }
            }

            long accumulator = 0;
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X + d.Y + d.Z + d.A + d.B + d.C + d.D + d.E;
            }
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X + d.Y + d.Z + d.A + d.B + d.C + d.D + d.E;
            }
            ReportMemory();
            _accumulator = accumulator;
        }

        private static void CopyClassTest()
        {
            var di = new PointClass[iterations];
            var data = new PointClass[iterations];

            for (var i = 0; i < iterations; i++)
            {
                di[i] = new PointClass(i, i + 1);
            }

            for (var z = 0; z < copyCount; z++)
            {
                for (var i = 0; i < iterations; i++)
                {
                    data[i] = di[i];
                }
            }

            long accumulator = 0;
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X + d.Y;
            }
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X + d.Y;
            }
            ReportMemory();
            _accumulator = accumulator;
        }

        private static void CopyClassWithRefsTest()
        {
            var di = new PointClassWithRefs[iterations];
            var data = new PointClassWithRefs[iterations];

            for (var i = 0; i < iterations; i++)
            {
                di[i] = new PointClassWithRefs(
	                new PointClass(i, i + 1),
	                new PointClass(i, i+1));
            }

            for (var z = 0; z < copyCount; z++)
            {
                for (var i = 0; i < iterations; i++)
                {
                    data[i] = di[i];
                }
            }

            long accumulator = 0;
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X.X + d.Y.Y;
            }
            for (var i = 0; i < iterations; i++)
            {
                var d = data[i];
                accumulator += d.X.X + d.Y.Y;
            }
            ReportMemory();
            _accumulator = accumulator;
        }

        public static void Test()
        {
            for( var i = 0; i < 5; i++ )
            {
                Console.WriteLine();
                Console.WriteLine( "Iteration - " + i );
                Measure( "Point Struct 2", TestStruct );
                Measure( "Point Class 2", TestClass );
                Measure( "Point Struct 3", Test3Struct );
                Measure( "Point Class 3", Test3Class );
                Measure( "Point Struct 4", Test4Struct );
                Measure( "Point Class 4", Test4Class );
                Measure( "Point Struct 8", Test8Struct );
                Measure( "Point Struct 3 Ref 1", Test3StructRef );
                Measure( "Point Class 3 Ref 1", Test3ClassRef );
                Measure( "Copy Struct 2", CopyStruct2Test );
                Measure( "Copy Struct 3", CopyStruct3Test );
                Measure( "Copy Struct 4", CopyStruct4Test );
                Measure( "Copy Struct 8", CopyStruct8Test );
                Measure( "Copy Class 2", CopyClassTest );

                Measure( "Point Struct With Refs", TestStructWithRefs );
                Measure( "Point Struct With 3 Refs", TestStructWith3Refs );
                Measure( "Point Class With Refs", TestClassWithRefs );
                Measure( "Copy Struct With Refs", CopyStructWithLinksTest );
                Measure( "Copy Struct With 3 Refs", CopyStructWith3LinksTest );
                Measure( "Copy Class With Refs", CopyClassWithRefsTest );
            }
        }
    }
}