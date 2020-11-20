/** @file cjt_clusters.cc
	@brief Código de la clase cjt_especies.
*/

#define _USE_MATH_DEFINES

#include "cjt_especies.hh"
#include <iostream>
#include <algorithm>
#include <cmath>

using namespace std;

/*void cjt_especies::union_interseccion_kmer(const vector<string>& kmer1, const vector<string>& kmer2, int& un, int& inter) {
	int i, j;
	un = inter = i = j = 0;

	while (i < kmer1.size() and j < kmer2.size()) {
		if (kmer1[i] == kmer2[j]) {
			++inter;
			++i;
			++j;
		}
		else if (kmer1[i] < kmer2[j]) ++i;
		else ++j;
		++un;
	}

	while (i < kmer1.size()) { //por si no se ha acabado de recorrer el conjunto kmer1 para la union
		++i;
		++un;
	}

	while (j < kmer2.size()) { //por si no se ha acabado de recorrer el conjunto kmer2 para la union
		++j;
		++un;
	}
}*/

void cjt_especies::prodEscalar_prodModulos(const vector<string>& kmer1, const vector<string>& kmer2, int& escalar, double& modulo) {
	int i, j, rep1, rep2; //rep1, rep2 son el numero de repeticiones de un kmer en los 2 vectores correspondeintes
	i = j = 0; 
	rep1 = rep2 = 1;
	escalar = 0;
	double mod1 = 0;
	double mod2 = 0;

	map<string, int> n_kmer1;
	map<string, int> n_kmer2;
	while (i < kmer1.size() and j < kmer2.size()) {
		if (i < kmer1.size() - 1 and kmer1[i] == kmer1[i+1]) ++rep1;
		else {
			mod1 += rep1*rep1;
			n_kmer1[kmer1[i]] = rep1;
			rep1 = 1;
		}
		if (j < kmer2.size() -1 and kmer2[j] == kmer2[j+1]) ++rep2;
		else {
			mod2 += rep2*rep2;
			n_kmer2[kmer2[j]] = rep2;
			rep2 = 1;
		}
		++i;
		++j;
	}

	while (i < kmer1.size()) { //por si no se ha acabado de recorrer el conjunto kmer1
		if (i < kmer1.size() -1 and kmer1[i] == kmer1[i+1]) ++rep1;
		else {
			mod1 += rep1*rep1;
			n_kmer1[kmer1[i]] = rep1;
			rep1 = 1;
		}
		++i;
	}

	while (j < kmer2.size()) { //por si no se ha acabado de recorrer el conjunto kmer2 
		if (j < kmer1.size() -1 and kmer2[j] == kmer2[j+1]) ++rep2;
		else {
			mod2 += rep2*rep2;
			n_kmer2[kmer2[j]] = rep2;
			rep2 = 1;
		}
		++j;
	}

	modulo = sqrt(mod1) * sqrt(mod2);

	map<string, int>::iterator it1 = n_kmer1.begin();
	map<string, int>::iterator it2 = n_kmer2.begin();

	while (it1 != n_kmer1.end() and it2 != n_kmer2.end()) {
		if (it1->first == it2->first) {
			escalar += it1->second * it2->second;
			++it1;
			++it2;
		}
		else if (it1->first < it2->first) ++it1;
		else ++it2;
	}
}

double cjt_especies::calcular_distancia(const string& id1, const string& id2) const{
	//int un, inter;

	vector<string> kmer1 = especies.at(id1).consultar_kmero();
	vector<string> kmer2 = especies.at(id2).consultar_kmero();
	//union_interseccion_kmer(kmer1, kmer2, un, inter);

	int escalar;
	double modulo;
	prodEscalar_prodModulos(kmer1, kmer2, escalar, modulo);
	return ((1-M_1_PI*acos(escalar/modulo))*100);
	
}

void cjt_especies::recalcular_distancias() {
	distancias.clear();
	for (map<string, Especie>::iterator it = especies.begin(); it != especies.end(); ++it) {
		for (map<string, Especie>::iterator it2 = it; it2 != especies.end(); ++it2) {
			string id1 = it->first;
			string id2 = it2->first;
			if (id1 == id2) distancias[id1][id2] = 0;
			else {
				double dist = calcular_distancia(id1, id2);
				distancias[id1][id2] = distancias[id2][id1] = dist;
			}
		}
	}
}

void cjt_especies::crear_especie(const string& id, const string& gen) {
    especies[id] = Especie(id, gen);
	for (map<string, Especie>::iterator it = especies.begin(); it != especies.end(); ++it) {
		string id2 = it->first;
		if (id2 == id) distancias[id][id] = 0;
		else {
			double dist = calcular_distancia(id, id2);
			distancias[id][id2] = distancias[id2][id] = dist;
		}
	}


	
}

void cjt_especies::eleminar_especie(const string& id){
	especies.erase(id);
	distancias.erase(id);
	for (map<string, Especie>::iterator it = especies.begin(); it != especies.end(); ++it) {
		string id2 = it->first;
		distancias[id2].erase(id);
	}
}

string cjt_especies::consultar_gen(const string& id) const {
	return especies.at(id).consultar_gen();
}

bool cjt_especies::existe_especie(const string& id) const {
	return especies.find(id) != especies.end();
}

double cjt_especies::consultar_distancia(const string& id1, const string& id2) const {
	return distancias.at(id1).at(id2);
}

void cjt_especies::leer_especies() {
	especies.clear();
	int n;
	cin >> n;
	for (int i = 0; i < n; ++i) {
		string id, gen;
		cin >> id >> gen;
		especies[id] = Especie(id, gen);
	}
	recalcular_distancias();
}

void cjt_especies::imprimir_especies() const {
	for (map<string, Especie>::const_iterator it = especies.begin(); it != especies.end(); ++it) {
		cout << it->first << " " << it->second.consultar_gen() << endl;
	}
}

void cjt_especies::imprimir_distancias() const {
	for (map<string, Especie>::const_iterator it = especies.begin(); it != especies.end(); ++it) {
		cout << it->first << ":";
		for (map<string, Especie>::const_iterator it2 = it; it2 != especies.end(); ++it2) {
			if (it != it2) {
				cout << " " << it2->first << " (" << distancias.at(it->first).at(it2->first) << ")";
			}
		}
		cout << endl;
	}
}

void cjt_especies::asignar_matriz(map<string, map<string, double> >& destino) const {
	destino = distancias;
}

void cjt_especies::asignar_cjt(map<string, Especie>& destino) const{
	destino = especies;
}
