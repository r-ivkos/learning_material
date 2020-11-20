/** @file cjt_cluster.hh
    @brief Especificaci�n de la clase cjt_cluster.
*/

#ifndef CJT_CLUSTERS_HH
#define CJT_CLUSTERS_HH
#include "cjt_especies.hh"
#include "Cluster.hh"

/** @class cjt_clusters
	
	@brief Representa la informaci�n y las operaciones asociadas a un conjunto de cl�sters, el cual est� formado a partir de 
	       un conjunto de especies, definido en el m�dulo cjt_especies.hh.

	Sus operaciones son las modificadoras que vac�an e inicializan de nuevo los cl�sters a partir de un conjunto de especies, y que fusionan 2 clusters de menor distancia en uno,
						las consultoras del n�mero de cl�sters en el conjunto y si existe un cl�ster.

*/

class cjt_clusters
{
private:
	/** @brief Un diccionario que almacena los cl�sters, con su identificador como clave.*/
	map<string, Cluster> clusters;
	
	/** @brief Matriz de distancias entre cl�sters que viene indexada por los identificadores de cada cluster*/
	map<string, map<string, double> > distancias;

public:

	/** @brief Inicializa el conjunto de cl�sters a partir de un conjunto de especies

		\pre <em>cierto</em>
		\post El par�metro impl�cito contiene cl�sters con identificadores de cada especie y las distancias entre cada cl�ster,
				que son las respectivas distancias entre especies.
	*/
	void inicializar(cjt_especies cesp);
	
	/** @brief Ejecuta un paso del algoritmo WPGMA y fusiona 2 cl�sters m�s cercanos

		\pre El par�metro impl�cito contiene como m�nimo 2 cl�sters
		\post 2 cl�sters del par�metro impl�cito m�s cercanos se han eliminado del conjunto y en su lugar se ha a�adido 
		       un cl�ster, en forma de �rbol, donde sus 2 subarboles son los cl�sters eliminados.  
	*/
	void paso_wpgma();

	void paso_upgma();

	/** @brief Ejecuta el algoritmo WPGMA formando un �rbol filogen�tico a partir del conjunto de especies

		\pre El conjunto de especies no esta vac�o.
		\post El par�metro impl�cito contiene un solo cl�ster con cl�sters de cada especie como hojas,
		      Lo imprime por el canal est�ndar de salida y tambi�n imprime las distancias entre 
			  cada cl�ster y sus hojas.
	*/
	void formar_arbol(cjt_especies cesp);

	/** @brief Dice si existe o no el cl�ster con ese identificador

		\pre <em>cierto</em>
		\post Devuelve true si el cl�ster con el identificador "id" esta al par�metro impl�cito, 
			  false si no esta.
	*/
	bool existe_cluster(const string& id) const;
	
	/** @brief Consulta el n�mero de clusters al conjunto
		
		\pre <em>cierto</em>
		\post Devuelve el n�mero de clusters al par�metro impl�cito
	*/
	int num_clusters() const;

	/** @brief Imprime el cl�ster deseado.

		\pre El par�metro impl�cito contiente el cl�ster con el identificador "id".
		\post Por el canal est�ndar de salida se ha imprimido el cl�ster.
    */
    void imprimir_cluster(const string& id) const;

	/** @brief Imprime el unico cluster del conjunto.

		\pre Al par�metro impl�cito hay exactamente un cl�ster que es el arbol que se ha formado al aplicar el algoritmo WPGMA.
		\post Por el canal est�ndar de salida se ha imprimido el cl�ster del par�metro impl�cito
	*/
	void imprimir_arbol() const;

	/** @brief Imprime la tabla de dist�ncias entre las especies del conjunto.

		\pre <em>cierto</em>
		\post Por el canal est�ndar de salida se ha escrito la tabla de dist�ncias de los cl�sters del par�metro impl�cito
	*/
	void imprimir_distancias() const;
};

#endif
