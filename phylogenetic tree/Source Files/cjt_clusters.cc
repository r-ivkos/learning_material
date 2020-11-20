/** @file cjt_clusters.cc 
	@brief Código de la clase cjt_clusters.
*/


#include "cjt_clusters.hh"
#include <iostream>
using namespace std;


void cjt_clusters::inicializar(cjt_especies cesp) {
	clusters.clear();
	cesp.asignar_matriz(distancias);
	map<string, Especie> especies;
	cesp.asignar_cjt(especies);
	for (map<string, Especie>::iterator it = especies.begin(); it != especies.end(); ++it) {
		clusters[it->first] = Cluster(it->first);
	}
}

void cjt_clusters::paso_wpgma()
{
	string clust1, clust2;
	double min_dist = 101; //la distancia maxima es 100, asi que siempre habra una menor a la 101
	for (map<string, Cluster>::iterator it = clusters.begin(); it != clusters.end(); ++it) { //encontrar 2 clusters mas cercanos
		for (map<string, Cluster>::iterator it2 = it; it2 != clusters.end(); ++it2) {
			if (it != it2) {
				double dist = distancias[it->first][it2->first];
				if (dist < min_dist) {
					min_dist = dist;
					clust1 = it->first;
					clust2 = it2->first;
				}
			}
		}
	}

	string new_clust = clust1+clust2;

	for (map<string, Cluster>::iterator it = clusters.begin(); it != clusters.end(); ++it) { //añadir las distancias entre el nuevo clústers y la resta 
		if (it->first != clust1 and it->first != clust2) {
			double dist = (distancias[clust1][it->first] + distancias[clust2][it->first])/2;
			distancias[new_clust][it->first] = distancias[it->first][new_clust] = dist;
			distancias[it->first].erase(clust1);
			distancias[it->first].erase(clust2);
		}
	}
	clusters[new_clust] = Cluster(clusters[clust1], clusters[clust2], distancias.at(clust1).at(clust2));
	clusters.erase(clust1);
	clusters.erase(clust2);

	distancias[new_clust][new_clust] = 0;
	distancias.erase(clust1);
	distancias.erase(clust2);

	

}

void cjt_clusters::paso_upgma() {
	string clust1, clust2;
	double min_dist = 101; //la distancia maxima es 100, asi que siempre habra una menor a la 101
	for (map<string, Cluster>::iterator it = clusters.begin(); it != clusters.end(); ++it) { //encontrar 2 clusters mas cercanos
		for (map<string, Cluster>::iterator it2 = it; it2 != clusters.end(); ++it2) {
			if (it != it2) {
				double dist = distancias[it->first][it2->first];
				if (dist < min_dist) {
					min_dist = dist;
					clust1 = it->first;
					clust2 = it2->first;
				}
			}
		}
	}

	string new_clust = clust1+clust2;

	for (map<string, Cluster>::iterator it = clusters.begin(); it != clusters.end(); ++it) { //añadir las distancias entre el nuevo clústers y la resta 
		if (it->first != clust1 and it->first != clust2) {
			double dist = (clusters[clust1].num_hojas() * distancias[clust1][it->first] + clusters[clust2].num_hojas() * distancias[clust2][it->first])/(clusters[clust1].num_hojas() + clusters[clust2].num_hojas());
			distancias[new_clust][it->first] = distancias[it->first][new_clust] = dist;
			distancias[it->first].erase(clust1);
			distancias[it->first].erase(clust2);
		}
	}
	clusters[new_clust] = Cluster(clusters[clust1], clusters[clust2], distancias.at(clust1).at(clust2));
	clusters.erase(clust1);
	clusters.erase(clust2);

	distancias[new_clust][new_clust] = 0;
	distancias.erase(clust1);
	distancias.erase(clust2);
}

void cjt_clusters::formar_arbol(cjt_especies cesp) {
	inicializar(cesp); 
	while (clusters.size() > 1) {
		paso_upgma();
	}
}

bool cjt_clusters::existe_cluster(const string& id) const {
	return clusters.find(id) != clusters.end();
}

int cjt_clusters::num_clusters() const {
	return clusters.size();
}

void cjt_clusters::imprimir_cluster(const string& id) const {
	Cluster::imprimir_cluster(clusters.at(id).consultar_cluster());
}

void cjt_clusters::imprimir_arbol() const
{
	imprimir_cluster(clusters.begin()->first);
}

void cjt_clusters::imprimir_distancias() const {
	for (map<string, Cluster>::const_iterator it = clusters.begin(); it != clusters.end(); ++it) {
		cout << it->first << ":";
		for (map<string, Cluster>::const_iterator it2 = it; it2 != clusters.end(); ++it2) {
			if (it != it2) {
				cout << " " << it2->first << " (" << distancias.at(it->first).at(it2->first) << ")";
			}
		}
		cout << endl;
	}
}
