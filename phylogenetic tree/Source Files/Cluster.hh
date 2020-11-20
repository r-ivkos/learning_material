/** @file Cluster.hh
	@brief Especificación de la clase Cluster.
*/

#ifndef CLUSTER_HH
#define CLUSTER_HH

#ifndef NO_DIAGRAM
#include "BinTree.hh"
#include <string>
#endif

using namespace std;

/** @struct Nodo 
	@brief Estructura de datos que representa el nodo de un Cluster.
*/
struct Nodo {
	/** @brief  La distancia entre el nodo del clúster y sus hojas. */
	double distancia_hojas;

	/** @brief El identificador del nodo. */
	string id;

	int num_hojas;
};

/** @class Cluster

	@brief Representa la información y las operaciones aplicables a un clúster.

	Sus operaciones son consultoras del cluster, la distancias entre el cluster y las constructoras a partir de una id y a partir de 2 clusters.

*/
class Cluster {
private:
	
	/** @brief el nodo de un clúster que almacena el identificador y la distancia entre el nodo del clúster y sus hojas*/
	Nodo nodo;
	/** @brief El nodo del clúster.*/
	
	/** @brief Árbol binario que representa un clúster, donde el nodo del árbol es el "Nodo" del clúster actual
	y sus hijos son los clústers, que estaban a la mínima distancia, que lo han formado.*/
	BinTree<Nodo> cluster;
	
public:

	/** @brief Consturctora por defecto de la clase Cluster.*/
	Cluster();

	/** @brief constructora de un clúster "hoja" a partir de un identificador.*/
	Cluster(const string& id);

	/** @brief constructora a partir de 2 clústers y la distnacia entre ellos.*/
	Cluster(const Cluster& clust1, const Cluster& clust2, double dist);


	/** @brief Consulta el árbol que forma el clúster.
	
		\pre <em>cierto</em>
		\post Devuelve el árbol del parámetro implícito.
	*/
	BinTree<Nodo> consultar_cluster() const;

	int num_hojas() const;

	/**  @brief Imprime el clúster deseado.
	
		\pre <em>cierto</em>
		\post Por el canal estándar de salida se ha escrito el clúster "clust". 
	*/
	static void imprimir_cluster(const BinTree<Nodo>& clust);
};


#endif
