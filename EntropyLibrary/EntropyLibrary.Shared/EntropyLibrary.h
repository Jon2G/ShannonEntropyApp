#pragma once
#include "EntropyLibrary.h"
#include <iostream>
#include <fstream>
#include <vector>
#include <math.h>
using std::cout;
using std::cerr;
using std::endl;
using std::string;
using std::ifstream;
using std::vector;
class Symbol
{
public:
	char Character;
	unsigned long Count;
	float Frecuency;
	Symbol(char Character);
};
class EntropyLibrary {
public:
	vector<Symbol*> Symbols;
	double TotalEntropy;
	Symbol* FindSymbol(vector<Symbol*> Symbols, char character);
	void CalculateTotalEntropy(EntropyLibrary* handle);
	EntropyLibrary();
	~EntropyLibrary();
};


extern "C" {
	char* getTemplateInfo();
	EntropyLibrary* GetEntropyLibrary();
	int ReadFromFile(EntropyLibrary* handle, const char* Filepath);
	double GetTotalEntropy(EntropyLibrary* handle);
	void ReleaseEntropyLibrary(EntropyLibrary* handle);

	unsigned long GetSymbolsLenght(EntropyLibrary* handle);
	///Arreglos proyectados
	float Calculate(float* EventsProbability, int size);
	char GetSymbolChar(EntropyLibrary* handle, unsigned long index);
	int GetSymbolCount(EntropyLibrary* handle, unsigned long index);
	float GetSymbolFrecuency(EntropyLibrary* handle, unsigned long index);

}




