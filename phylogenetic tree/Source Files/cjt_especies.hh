/** @file cjt_especies.hh
	@brief Especificaci�n de la clase cjt_especies
*/

#ifndef CJT_ESPECIES_HH
#define CJT_ESPECIES_HH

#ifndef NO_DIAGRAM
#include <map>
#include <string>
#include <vector>
#endif
#include "Especie.hh"

using namespace std;

/** @class cjt_especies

	@brief  Representa la informaci�n y las operaciones asociadas a un conjunto de especies.

	Sus operaciones son las modificadoras que a�aden o eliminan una especie del conjunto y la reescribe el conjunto con nuevas especies, insertadas por el usuario, 
						las consultoras de si existe una especie al conjunto y para obtener el gen de una especie.

*/

class cjt_especies
{

private:

	/** @brief diccionario que guarda especies, tomando el identificador como clave y la clase Especie como su valor. */
    map<string, Especie> especies; 

	/** @brief una matriz, indexada con identificadores de especies (de tipo "string"), de distancias. */
	map<string, map<string, double> > distancias; 

	

	/** @breif Calcula el n�mero de elementos del conjunto intersecci�n de los conujuntos de subsecuencias y
			   Numero de elementos del conjunto de uni�n de los conjuntos de subsecuencias.

		\pre "kmer1" y "kmer2" no est�n vac�os y tienen subsecuencias de genes.
		\post "un" = cardinal del conjunto uni�n de "kmer1" y "kmer2"; y "inter" cardinal del conjunto instersecci�n.
	*/
	//static void union_interseccion_kmer(const vector<string>& kmer1, const vector<string>& kmer2, int& un, int& inter);

	static void prodEscalar_prodModulos(const vector<string>& kmer1, const vector<string>& kmer2, int& escalar, double& modulo);

	/** @beif Calcula la distancia entre 2 especies del conjunto.

		\pre El par�metro impl�cito contiene las especies con los identificadores "id1" y "id2" y son diferentes.
		\post Devuelve la distancia entre las especies con los identificadores "id1" y "id2".
	*/
	double calcular_distancia(const string& id1, const string& id2) const;

	/** @brief Calcula y guarda las distancias entre todas las especies del conjunto.

		\pre <em>cierto</em>
		\post El par�metro impl�cito "distancias" contiene la tabla de distancias entre especies,
	*/
	void recalcular_distancias();

public:

	/** @brief A�ade la especie al conjunto.

		\pre La especie con el identificador "id" no existe al par�metro impl�cito.
		\post El par�metro impl�cito queda actualizado, conserva todas las especies anteriores m�s
			la especie con el identificador "id" y gen "gen". Tambi�n se han a�adido las distancias entre la especie
			nueva y las especies ya existentes.
	*/
	void crear_especie(const string& id, const string& gen);
	
	/** @brief Borra la especie del conjunto.

		\pre La especie con el identificador "id" existe al par�metro impl�cito.
		\post Se ha eliminado la especie con el identificador "id" del par�metro impl�cito. Tambi�n se han eliminado
				todas las distancias entre la especie eliminada y la resta de especies
	*/
	void eleminar_especie(const string& id);

	/** @brief Devuelve el gen de la especie solicitada.

		\pre El par�metro impl�cito contiene la especie con el identificador "id.
		\post Devuelve strings, que es el gen de la especie con el identificador "id".
	*/
	string consultar_gen(const string& id) const;

	/** @brief Devuelve si existe o no una especie.

		\pre <em>cierto</em>
		\post Devuelve un booleano con valor "true" si existe la especie con el identificador "id" y
			   false si no existe.
	*/
	bool existe_especie(const string& id) const;

	/** @brief Devueve la distancia entre 2 especies del conjunto.

		\pre El par�metro impl�cito contiene especies con identificadores id1 y id2.
		\post Devuelve un double, que es la distancia entre la especie con el identificador "id1" y 
		       especie con el identificador "id2".
	*/
	double consultar_distancia(const string& id1, const string& id2) const;

	/** @brief Lee las especies por el canal est�ndar de entrada.

		\pre <em>cierto</em>
		\post El par�metro impl�cito tiene las n especies le�das, donde n es un entero >= 0.
	*/
	void leer_especies();

	/** @brief Imprime todas las especies y sus genes del conjunto.

		\pre <em>cierto</em>
		\post Por el canal est�ndar de salida se han escrito todas las especies del par�metro impl�cito.
	*/
	void imprimir_especies() const;

	/** @brief Imprime la tabla de distancias entre las especies del conjunto.

		\pre <em>cierto</em>
		\post Por el canal est�ndar de salida se ha escrito la tabla de distancias de las especies del par�metro impl�cito
	*/
	void imprimir_distancias() const;

	/** @brief Le asigna los valores de la matriz de distancias a la matriz pasada
	
		\pre <em>cierto</em>
		\post El par�metro "destino" contiene los valores del par�metro impl�cito
	*/
	void asignar_matriz(map<string, map<string, double> >& destino) const;

	/** @brief Le asigna los valores del conjunto de especies al conjunto pasado
	
		\pre <em>cierto<em>
		\post El pr�metro "destino" contiene los valores del par�metro impl�cito
	*/
	void asignar_cjt(map<string, Especie>& destino) const;
};

#endif

