﻿using System;
using HermiteInterpolation.Functions;
using HermiteInterpolation.Shapes.HermiteSpline.Bicubic;
using HermiteInterpolation.SplineKnots;
using HermiteInterpolation.Utils;

namespace HermiteInterpolation.Shapes.HermiteSpline.Biquartic
{
    public class BiquarticMeshGenerator : BicubicMeshGenerator
    {
        private class BiquarticKnotsGenerator : KnotsGenerator
        {
            private KnotsGenerator _baseGenerator;

            public BiquarticKnotsGenerator(KnotsGenerator baseGenerator)
                : base(baseGenerator.Function)
            {
                _baseGenerator = baseGenerator;
            }

            public override Knot[][] GenerateKnots(SurfaceDimension uDimension, SurfaceDimension vDimension)
            {
                var afvOld = _baseGenerator.GenerateKnots(uDimension, vDimension);
                var uCountOld = afvOld.Length;
                var vCountOld = afvOld[0].Length;
                var uCountNew = (2 * uCountOld - 1);
                var vCountNew = (2 * vCountOld - 1);
                var afvNew = MyArrays.JaggedArray<Knot>(uCountNew, vCountNew);

                FillNewKnots(afvNew, afvOld);

                return afvNew;
            }
      
            private void FillNewKnots(Knot[][] newKnots,
                Knot[][] oldKnots)
            {
                // z, dx, dy, dxy initialization
                OriginalKnots(newKnots, oldKnots);
                // z, dx, deltay, deltaxy
                VerticalKnots(newKnots);
                // z, Dx, dy, Dxy
                HorizontalKnots(newKnots);
                // z, Dx, deltay, deltaxy
                InnerKnots(newKnots);
            }

            private void InnerKnots(Knot[][] newKnots)
            {
                for (int i = 1; i < newKnots.Length; i += 2)
                {
                    for (int j = 1; j < newKnots[0].Length; j += 2)
                    {
                        var u0v0 = newKnots[i - 1][j - 1];
                        var u1v0 = newKnots[i][j - 1];
                        var u2v0 = newKnots[i + 1][j - 1];

                        var u0v1 = newKnots[i - 1][j];
                        var u1v1 = new Knot();
                        var u2v1 = newKnots[i + 1][j];

                        var u0v2 = newKnots[i - 1][j + 1];
                        var u1v2 = newKnots[i][j + 1];
                        var u2v2 = newKnots[i + 1][j + 1];

                        var hx = Math.Abs(u2v0.X - u0v0.X) / 2;
                        var hx4 = hx * 4;
                        var hy = Math.Abs(u0v2.Y - u0v0.Y) / 2;
                        var hy4 = hy * 4;
                        var x = u0v1.X + hx;
                        var y = u1v0.Y + hy;

                        u1v1.X = x;
                        u1v1.Y = y;
                        u1v1.Z = Function.Z(x, y);
                        u1v1.Dx = -(3 * (u0v1.Z - u2v1.Z) + hx * (u0v1.Dx + u2v1.Dx)) /
                                 hx4;
                        u1v1.Dy = -(3 * (u1v0.Z - u1v2.Z) + hx * (u1v0.Dx + u1v2.Dx)) /
                                  hy4;
                        u1v1.Dxy = (u0v0.Dxy + u2v0.Dxy + u0v2.Dxy + u2v2.Dxy +
                                    (3 *
                                     ((u0v0.Dy - u2v0.Dy + u0v2.Dy - u2v2.Dy) / hx +
                                      (u0v0.Dx + u2v0.Dx - u0v2.Dx - u2v2.Dx) / hy +
                                      (3 * (u0v0.Z - u2v0.Z - u0v2.Z + u2v2.Z)) / (hx * hy)))) /
                                   16;
                        newKnots[i][j] = u1v1;
                    }
                }
            }

            private void HorizontalKnots(Knot[][] newKnots)
            {
                for (int i = 1; i < newKnots.Length; i += 2)
                {
                    for (int j = 0; j < newKnots[0].Length; j += 2)
                    {

                        var u0 = newKnots[i - 1][j];
                        var u1 = new Knot();
                        // finished
                        var u2 = newKnots[i + 1][j];

                        var hx = Math.Abs(u2.X - u0.X) / 2;
                        var hx4 = hx * 4;
                        var x = u0.X + hx;
                        var y = u0.Y;

                        u1.X = x;
                        u1.Y = y;
                        u1.Z = Function.Z(x, y);

                        u1.Dy = Function.Dy(x, y);
                        u1.Dx = -(3 * (u0.Z - u2.Z) + hx * (u0.Dx + u2.Dx)) /
                                hx4;
                        u1.Dxy = -(3 * (u0.Dy - u2.Dy) + hx * (u0.Dxy + u2.Dxy)) /
                                 hx4;
                        // finished
                        newKnots[i][j] = u1;
                    }
                }
            }

            private void VerticalKnots(Knot[][] newKnots)
            {

                for (int i = 0; i < newKnots.Length; i += 2)
                {
                    for (int j = 1; j < newKnots[0].Length; j += 2)
                    {

                        var v0 = newKnots[i][j - 1];
                        var v1 = new Knot();
                        // finished
                        var v2 = newKnots[i][j + 1];

                        var hy = Math.Abs(v2.Y - v0.Y) / 2;
                        var hy4 = hy * 4;
                        var x = v0.X;
                        var y = v0.Y + hy;

                        v1.X = x;
                        v1.Y = y;
                        v1.Z = Function.Z(x, y);
                        v1.Dx = Function.Dx(x, y);
                        v1.Dy = -(3 * (v0.Z - v2.Z) + hy * (v0.Dy + v2.Dy)) /
                                hy4;
                        v1.Dxy = -(3 * (v0.Dx - v2.Dx) + hy * (v0.Dxy + v2.Dxy)) /
                                 hy4;
                        // finished
                        newKnots[i][j] = v1;
                    }
                }
            }

            private static void OriginalKnots(Knot[][] newKnots, Knot[][] oldKnots)
            {
                for (var i = 0; i < oldKnots.Length; i++)
                {
                    for (var j = 0; j < oldKnots[0].Length; j++)
                    {
                        newKnots[2 * i][2 * j] = oldKnots[i][j];
                    }
                }
            }
        }

       
        public BiquarticMeshGenerator(SurfaceDimension uDimension, SurfaceDimension vDimension, InterpolatedFunction function,
            KnotsGenerator knotsGenerator, Derivation derivation)
            : base(uDimension, vDimension, new BiquarticKnotsGenerator(knotsGenerator), derivation)
       
        {
           
        }

    }
}