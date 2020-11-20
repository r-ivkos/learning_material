/** @file Especie.hh
	@brief Especificaci�n de la clase Especie.
*/

#ifndef ESPECIE_HH
#define ESPECIE_HH

#ifndef NO_DIAGRAM
#include <string>
#include <vector>
#endif

using namespace std;

/** @class Especie
	@brief epresenta la informaci�n y las operaciones aplicables a una especie.

	Sus operaciones son consultoras de gen de la especie, de su kamero, y una modificadora del valor k.
*/

class Especie {
private:
	/** @brief El valor para el c�lculo de distancias entre especies, concretamente para el c�lculo de un conjunto k-mero. */
	static int k;

	/** @brief El identificador de la especie.*/
	string id;
	
	/** @brief El gen de la especie.*/
	string gen;

	/** @brief Almacena las subsecuencias de longitud k del gen de la especie.*/
	vector<string> k_mero;
	
	/** @brief Divide el gen de la especie en subsecuencias de longitud "k".

		\pre k >= 1
		\post El par�metro impl�cito contiene subsecuencias de longitud "k" del gen de la especie
			   ordenado crecientemente en el orden lexicogr�fico.
	*/
	void kmero();

public:
	/** @brief La constructora por defecto.*/
	Especie();

	/** @brief La constructora de la clase Especie a partir de un identificador y un gen.*/
	Especie(string Id, string Gen);

	/** @brief Devuelve el gen de la especie.

		\pre <em>cierto</em>
		\post Devuelve un string, que es el gen de la especie.
	*/
	string consultar_gen() const;

	/** @brief Devuelve el kmero de la especie.
	
		\pre <em>cierto</em>
		\post Devuelve un vector, que es el conjunto de subsecuencias del gen del parametro implicito de longitud k.
	*/
	vector<string> consultar_kmero() const;

	/** @breif Lee el valor k >= 1 que servira para calcular distancias entre especies.

		\pre <em>cierto</em>
		\post Ha le�do el valor k del canal est�ndar de entrada.
    */
	static void leer_k();
};

#endif

