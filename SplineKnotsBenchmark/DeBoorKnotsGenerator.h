#pragma once
#include "KnotsGenerator.h"
#include "SurfaceDimension.h"
#include <functional>
#include <vector>

namespace splineknots
{
	typedef std::function<double(int)> RightSideSelector;
	typedef std::function<void(int, double)> UnknownsSetter;
	class DeBoorKnotsGenerator : public KnotsGenerator
	{
		
	public:
		DeBoorKnotsGenerator(MathFunction math_function);
		DeBoorKnotsGenerator(InterpolativeMathFunction math_function);
		~DeBoorKnotsGenerator();

		virtual std::unique_ptr<KnotMatrix> GenerateKnots(SurfaceDimension& udimension, SurfaceDimension& vdimension);
	protected:
		virtual std::vector<double> MainDiagonal(size_t unknowns_count);
		virtual std::vector<double> LowerDiagonal(size_t unknowns_count);
		virtual std::vector<double> UpperDiagonal(size_t unknowns_count);

		virtual std::vector<double> RightSide(RightSideSelector& right_side_autoiables, double h, double dfirst, double dlast,
		                                      int unknowns_count);
		virtual void InitializeKnots(SurfaceDimension& udimension, SurfaceDimension& vdimension, KnotMatrix& values);
		virtual void FillXDerivations(KnotMatrix& values);
		virtual void FillXYDerivations(KnotMatrix& values);
		virtual void FillYDerivations(KnotMatrix& values);
		virtual void FillYXDerivations(KnotMatrix& values);
		virtual void FillXDerivations(int column_index, KnotMatrix& values);
		virtual void FillXYDerivations(int column_index, KnotMatrix& values);
		virtual void FillYDerivations(int row_index, KnotMatrix& values);
		virtual void FillYXDerivations(int row_index, KnotMatrix& values);
		virtual void SolveTridiagonal(RightSideSelector& selector, double h, double dfirst, double dlast,
		                              int unknowns_count, UnknownsSetter& unknowns_setter);
	};
}
