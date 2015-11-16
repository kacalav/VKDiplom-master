#pragma once
#include "KnotMatrix.h"
#include "MathFunction.h"
#include "SurfaceDimension.h"

namespace splineknots {
	
	//double safeCall(MathFunction function, double x, double y);

	class KnotsGenerator
	{
		InterpolativeMathFunction function_;
	public:
	
		virtual ~KnotsGenerator();// = default;
		const InterpolativeMathFunction& Function() const;

		virtual KnotMatrix GenerateKnots(SurfaceDimension& udimension, SurfaceDimension& vdimension) = 0;
	protected:
		KnotsGenerator(MathFunction function);
		KnotsGenerator(InterpolativeMathFunction function);
		//virtual ~KnotsGenerator();
		
	};

}

