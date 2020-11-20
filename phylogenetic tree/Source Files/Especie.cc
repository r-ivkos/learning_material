/** @file Especie.cc
	@breif código de la calse Especie
*/

#include "Especie.hh"
#include <algorithm>
#include <iostream>

int Especie::k = 1;

void Especie::kmero() {
	int s = gen.size();
	k_mero = vector<string>(s+1-k);
	for (int i = 0; i < s + 1 - k; ++i) {
		string subsec;
		for (int j = i; j <i+k; ++j) {
			subsec += gen[j];
		}
		k_mero[i] = subsec;
	}
	sort(k_mero.begin(), k_mero.end());
}

Especie::Especie() {
	
}

Especie::Especie(string Id, string Gen) {
	id = Id;
	gen = Gen;
	kmero();
}

string Especie::consultar_gen() const {
	return gen;
}

vector<string> Especie::consultar_kmero() const {
	return k_mero;
}

void Especie::leer_k() {
	cin >> k;
}
