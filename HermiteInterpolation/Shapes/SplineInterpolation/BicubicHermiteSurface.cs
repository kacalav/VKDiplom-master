﻿using System; using HermiteInterpolation.Numerics; using HermiteInterpolation.Numerics.MathFunctions; using HermiteInterpolation.Primitives; using HermiteInterpolation.SplineKnots; using MathNet.Numerics.LinearAlgebra; using Microsoft.Xna.Framework;  namespace HermiteInterpolation.Shapes.SplineInterpolation {     public class BicubicHermiteSurface :Spline     {          public BicubicHermiteSurface(SurfaceDimension uDimension, SurfaceDimension vDimension, MathExpression mathExpression, Derivation derivation = Derivation.Zero) : base(uDimension, vDimension, mathExpression, derivation)         {         }          public BicubicHermiteSurface(SurfaceDimension uDimension, SurfaceDimension vDimension, InterpolativeMathFunction interpolativeMathFunction, Derivation derivation = Derivation.Zero) : base(uDimension, vDimension, interpolativeMathFunction, derivation)         {         }          public BicubicHermiteSurface(SurfaceDimension uDimension, SurfaceDimension vDimension, KnotsGenerator knotsGenerator, Derivation derivation = Derivation.Zero) : base(uDimension, vDimension, knotsGenerator, derivation)         {         }          protected delegate Vector<double> BasisVector(double t, double t0, double t1);          /// <summary>         ///          /// </summary>         /// <param name="uIdx"></param>         /// <param name="vIdx"></param>         /// <param name="knots"></param>         /// <returns></returns>         protected override ISurface CreateSegment(int uIdx, int vIdx, KnotMatrix knots)         {                          var u0 = knots[uIdx,vIdx].X;             var u1 = knots[uIdx+1,vIdx].X;             var v0 = knots[uIdx,vIdx].Y;             var v1 = knots[uIdx,vIdx+1].Y;             var meshDensity = Constants.MeshDensity;             var uKnotsDistance = Math.Abs(u1-u0);                        var xCount = Math.Ceiling(uKnotsDistance/meshDensity);                    var yKnotDistance =  Math.Abs(v1 - v0);             var yCount = Math.Ceiling(yKnotDistance / meshDensity);                       var verticesCount = (int)((++xCount) * (++yCount));              var segmentMeshVertices = new VertexPositionNormalColor[verticesCount];             var basisCalculator = new BicubicBasis(knots,Derivation);//BasisFactory.CreateBasis(Surface.Type);                          var phi = basisCalculator.Matrix(uIdx, vIdx);             var k = 0;             var x = (float) u0 ;             for (var i = 0; i < xCount; i++,x += meshDensity)             {                 x = x < u1 ? x : (float)u1;                 var lambda1 = basisCalculator.Vector(x, u0, u1);                 var basis = phi.LeftMultiply(lambda1);                        var y = (float) v0;                 for (var j = 0; j < yCount; j++,y += meshDensity)                 {                     y = y < v1 ? y : (float)v1;                     var lambda2 = basisCalculator.Vector(y, v0, v1);                     var zv = basis.PointwiseMultiply(lambda2);                     var z = (float) zv.Sum();                     z = MathHelper.Clamp(z, -Constants.MaxSceneZDifference, Constants.MaxSceneZDifference);                        segmentMeshVertices[k++] = new VertexPositionNormalColor(new Vector3(x, y, z), DefaultNormal,                         DefaultColor);                                    }                             }                         return new SimpleSurface(segmentMeshVertices, (int)xCount, (int)yCount);                    }     } }