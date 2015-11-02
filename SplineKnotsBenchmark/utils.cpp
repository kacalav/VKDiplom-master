#include "stdafx.h"
#include "utils.h"


namespace utils
{
	/*template<typename T>
	T* utils::initArray(size_t length, T* arrayToInit, T& value)
	{
		for (size_t i = 0; i < length; i++)
		{
			arrayToInit[i] = std::copy(value);
		}
		return arrayToInit;
	}*/


	template <typename T>
	T* initArray(size_t length, T* arrayToInit, T value)
	{
		for (size_t i = 0; i < length; i++)
		{
			arrayToInit[i] = std::copy(value);
		}
		return arrayToInit;
	}

	template<typename T>
	void DeleteJaggedArray(T**& jaggedArray, size_t rows, size_t columns)
	{
		for (size_t i = 0; i < rows; i++)
		{
			delete[] jaggedArray[i];
		}
		delete[] jaggedArray;
		jaggedArray = nullptr;
	}

	template <typename T>
	T** CreateJaggedArray(size_t rows, size_t columns)
	{
		auto res = new T*[rows];
		for (size_t i = 0; i < columns; i++)
		{
			res = new T[columns];
		}
		
		return res;
	}
}
