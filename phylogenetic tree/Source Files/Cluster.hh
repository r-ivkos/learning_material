/** @file Cluster.hh
	@brief Especificaci�n de la clase Cluster.
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
	/** @brief  La distancia entre el nodo del cl�ster y sus hojas. */
	double distancia_hojas;

	/** @brief El identificador del nodo. */
	string id;

	int num_hojas;
};

/** @class Cluster

	@brief Representa la informaci�n y las operaciones aplicables a un cl�ster.

	Sus operaciones son consultoras del cluster, la distancias entre el cluster y las constructoras a partir de una id y a partir de 2 clusters.

*/
class Cluster {
private:
	
	/** @brief el nodo de un cl�ster que almacena el identificador y la distancia entre el nodo del cl�ster y sus hojas*/
	Nodo nodo;
	/** @brief El nodo del cl�ster.*/
	
	/** @brief �rbol binario que representa un cl�ster, donde el nodo del �rbol es el "Nodo" del cl�ster actual
	y sus hijos son los cl�sters, que estaban a la m�nima distancia, que lo han formado.*/
	BinTree<Nodo> cluster;
	
public:

	/** @brief Consturctora por defecto de la clase Cluster.*/
	Cluster();

	/** @brief constructora de un cl�ster "hoja" a partir de un identificador.*/
	Cluster(const string& id);

	/** @brief constructora a partir de 2 cl�sters y la distnacia entre ellos.*/
	Cluster(const Cluster& clust1, const Cluster& clust2, double dist);


	/** @brief Consulta el �rbol que forma el cl�ster.
	
		\pre <em>cierto</em>
		\post Devuelve el �rbol del par�metro impl�cito.
	*/
	BinTree<Nodo> consultar_cluster() const;

	int num_hojas() const;

	/**  @brief Imprime el cl�ster deseado.
	
		\pre <em>cierto</em>
		\post Por el canal est�ndar de salida se ha escrito el cl�ster "clust". 
	*/
	static void imprimir_cluster(const BinTree<Nodo>& clust);
};


#endif
