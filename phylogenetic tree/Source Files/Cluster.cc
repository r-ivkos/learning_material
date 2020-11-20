/** @file Cluster.cc
	@brief Código de la clase cjt_especies. 
*/

#include "Cluster.hh"
#include <iostream>
using namespace std;



Cluster::Cluster() {}

Cluster::Cluster(const string& id)
{
	nodo.id = id;
	nodo.distancia_hojas = -1; //sera el valor por defecto de una hoja
	cluster = BinTree<Nodo>(nodo);
	nodo.num_hojas = 1;
}

Cluster::Cluster(const Cluster& clust1, const Cluster& clust2, double dist)
{
	nodo.id = clust1.nodo.id + clust2.nodo.id;
	nodo.num_hojas = clust1.nodo.num_hojas + clust2.nodo.num_hojas;
	nodo.distancia_hojas = dist/2;
	cluster = BinTree<Nodo>(nodo, clust1.cluster, clust2.cluster);
	
}

BinTree<Nodo> Cluster::consultar_cluster() const {
	return cluster;
}

int Cluster::num_hojas() const
{
	return nodo.num_hojas;
}

void Cluster::imprimir_cluster(const BinTree<Nodo>& clust)
{
	if (not clust.empty()) {
		cout << "[";
		if (clust.value().distancia_hojas != -1) {
			cout << "(" << clust.value().id << ", " << clust.value().distancia_hojas << ") ";
		}
		else cout << clust.value().id;
		imprimir_cluster(clust.left());
		imprimir_cluster(clust.right());
		cout << "]";
	}
}


/*BinTree<Cluster> Cluster::consultar_cluster() const
{
	return BinTree<Cluster>();
}*/

/*double Cluster::consultar_distancia() const
{
	return 0.0;
}

void Cluster::imprimir_cluster(const BinTree<string>& clust)
{
	if (clust.left().empty() and clust.right().empty()) {
		cout << clust.value();
	}
	else {
		cout << '{';
		imprimir_cluster(clust.left());
		cout << ',';
		imprimir_cluster(clust.right());
		cout << '}';
	}
}*/
