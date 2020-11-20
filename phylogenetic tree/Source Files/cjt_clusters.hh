/** @file cjt_cluster.hh
    @brief Especificación de la clase cjt_cluster.
*/

#ifndef CJT_CLUSTERS_HH
#define CJT_CLUSTERS_HH
#include "cjt_especies.hh"
#include "Cluster.hh"

/** @class cjt_clusters
	
	@brief Representa la información y las operaciones asociadas a un conjunto de clústers, el cual está formado a partir de 
	       un conjunto de especies, definido en el módulo cjt_especies.hh.

	Sus operaciones son las modificadoras que vacían e inicializan de nuevo los clústers a partir de un conjunto de especies, y que fusionan 2 clusters de menor distancia en uno,
						las consultoras del número de clústers en el conjunto y si existe un clúster.

*/

class cjt_clusters
{
private:
	/** @brief Un diccionario que almacena los clústers, con su identificador como clave.*/
	map<string, Cluster> clusters;
	
	/** @brief Matriz de distancias entre clústers que viene indexada por los identificadores de cada cluster*/
	map<string, map<string, double> > distancias;

public:

	/** @brief Inicializa el conjunto de clústers a partir de un conjunto de especies

		\pre <em>cierto</em>
		\post El parámetro implícito contiene clústers con identificadores de cada especie y las distancias entre cada clúster,
				que son las respectivas distancias entre especies.
	*/
	void inicializar(cjt_especies cesp);
	
	/** @brief Ejecuta un paso del algoritmo WPGMA y fusiona 2 clústers más cercanos

		\pre El parámetro implícito contiene como mínimo 2 clústers
		\post 2 clústers del parámetro implícito más cercanos se han eliminado del conjunto y en su lugar se ha añadido 
		       un clúster, en forma de árbol, donde sus 2 subarboles son los clústers eliminados.  
	*/
	void paso_wpgma();

	void paso_upgma();

	/** @brief Ejecuta el algoritmo WPGMA formando un árbol filogenético a partir del conjunto de especies

		\pre El conjunto de especies no esta vacío.
		\post El parámetro implícito contiene un solo clúster con clústers de cada especie como hojas,
		      Lo imprime por el canal estándar de salida y también imprime las distancias entre 
			  cada clúster y sus hojas.
	*/
	void formar_arbol(cjt_especies cesp);

	/** @brief Dice si existe o no el clúster con ese identificador

		\pre <em>cierto</em>
		\post Devuelve true si el clúster con el identificador "id" esta al parámetro implícito, 
			  false si no esta.
	*/
	bool existe_cluster(const string& id) const;
	
	/** @brief Consulta el número de clusters al conjunto
		
		\pre <em>cierto</em>
		\post Devuelve el número de clusters al parámetro implícito
	*/
	int num_clusters() const;

	/** @brief Imprime el clúster deseado.

		\pre El parámetro implícito contiente el clúster con el identificador "id".
		\post Por el canal estándar de salida se ha imprimido el clúster.
    */
    void imprimir_cluster(const string& id) const;

	/** @brief Imprime el unico cluster del conjunto.

		\pre Al parámetro implícito hay exactamente un clúster que es el arbol que se ha formado al aplicar el algoritmo WPGMA.
		\post Por el canal estándar de salida se ha imprimido el clúster del parámetro implícito
	*/
	void imprimir_arbol() const;

	/** @brief Imprime la tabla de distáncias entre las especies del conjunto.

		\pre <em>cierto</em>
		\post Por el canal estándar de salida se ha escrito la tabla de distáncias de los clústers del parámetro implícito
	*/
	void imprimir_distancias() const;
};

#endif
