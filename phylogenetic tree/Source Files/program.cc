/** @mainpage
	
	Es la documentación de la práctica de Ivan Kosovtsev.

	El programa principal se encuentra en el módulo program.cc.
	Atendiendo a los tipos de datos sugeridos en el enunciado, necesitaremos
	un módulo para representar una especie, uno para reprsentar un cluster, uno para reprsentera un conjunto de especies y otro para representar
	el conjunto de clústers.

*/

/** @file program.cc

    @brief El programa principal

	Estamos suponiendo que los datos leídos siempre son correctos, ya que
	no incluimos comprobaciones al respecto. Para facilitar el uso de las funciones
	del conjunto de especies, el valor k >=0, que nos sera útil para calcular distancias
	entre especies, es una variable estática del dicho conjunto, y se lee desde la función
	del mismo.

	El programa cumple con las siguientes funciones que podra ejecutar el usuario:

	1. crea_especie: Crea una especie con el identificador y gen (dos strings) dados. Escribe un mensaje de error si ya existe una especie con el mismo identificador. La especie creada, si no hay error, se agrega al conjunto de especies.
	2. obtener_gen: Dado un identificador de especie, imprime el gen asociado a la especie. Escribe un mensaje de error si no existe una especie con el identificador dado.
	3. distancia: Dados dos identificadores de especies, imprime la distancia entre las dos especies. Se escribe un mensaje de error si alguna de las dos especies cuyos identificadores se dan no existen. 
	4. elimina_especie: Dado el identificador de una especieela elimina del conjunto de especies. Escribe un mensaje de error si la especie con el identificador dado no existe.
	5. existe_especie: Dado el identificador de una especie e imprime una indicación de si dicha especie existe (es decir, es parte del conjunto de especies).
	6. lee_cjt_especies: Lee del canal estándar de entrada un entero n≥0 y a continuación una secuencia de n especies (pares identificador-gen). Las n especies dadas tienen identificadores distintos entre sí. Los contenidos previos del conjunto de especies se descartan las especies dejan de existir y las n especies leídas se agregan al conjunto de especies.
	7. imprime_cjt_especies: Imprime en el canal estándar de salida el conjunto de especies. Si el conjunto es vacío, no imprime ninguna información.
	8. tabla_distancias: Imprime la tabla de distancias entre cada par de especies del conjunto de especies. Si el conjunto es vacío, no imprime ninguna información.
	9. inicializa_clusters: Inicializa el conjunto de clústers con el conjunto de especies en el estado en el que esté en ese momento, e imprime la tabla de distancias entre clústers. Si el conjunto es vacío, no imprime ninguna información.
	10. ejecuta_paso_wpgma: ejecuta un paso del algoritmo WPGMA (fusiona los dos clústers a menor distancia en uno nuevo) e imprime la tabla de distancias entre clústers resultante. En caso de que el número de clústers del conjunto sea menor o igual que uno, solamentese imprimie un mensaje de error.
	11. imprime_cluster: dado un identificador α, imprime el clúster (su “estructura arbores-cente”) con el identificador dado, o un error si no existe un clúster con dicho identificador en el conjunto de clústers.
	12. imprime_arbol_filogenetico: imprime el árbol filogenético para el conjunto de especies actual; dicho árbol es el clúster que agrupa todas las especies, resultante de aplicar el algoritmo WPGMA. El contenido del conjunto de clústers previo se descarta y se reinicializa con el conjunto de especies en el estado en el que esté en ese momento, para acontinuación aplicar el algoritmo. El conjunto de clústers final es el que queda después de aplicar el algoritmo. Si el nuevo conjunto de clústers es vacío, solamente se escribirá un mensaje de error.
	13. fin: finaliza la ejecución del programa
*/

#ifndef NO_DIAGRAM
#include <iostream>
#endif
#include "cjt_clusters.hh"

using namespace std;


int main() {

	Especie::leer_k();

	string opcion; //Código de operación

	cjt_especies cesp; //El conjunto de especies con el que se trabajara
	cjt_clusters clust; //El conjunto de clusters con el que se trabajara

	while (cin >> opcion and opcion != "fin") { 

		if (opcion == "crea_especie") {
			string id, gen;
			cin >> id >> gen;
			cout << "# crea_especie " << id << " " << gen << endl;
			if (cesp.existe_especie(id)) cout << "ERROR: La especie " << id << " ya existe." << endl;
			else cesp.crear_especie(id, gen);
			cout << endl;
		}

		else if (opcion == "obtener_gen") {
			string id;
			cin >> id;
			cout << "# obtener_gen " << id << endl;
			if (not cesp.existe_especie(id)) cout << "ERROR: La especie " << id << " no existe." << endl << endl;
			else cout << cesp.consultar_gen(id) << endl << endl;
			
		}

		else if (opcion == "distancia") {
			string id1, id2;
			cin >> id1 >> id2;
			cout << "# distancia " << id1 << " " << id2 << endl;
			if (not cesp.existe_especie(id1) and cesp.existe_especie(id2)) 
				cout << "ERROR: La especie " << id1 << " no existe." << endl << endl;

			else if (cesp.existe_especie(id1) and not cesp.existe_especie(id2)) 
				cout << "ERROR: La especie " << id2 << " no existe." << endl << endl;

			else if (not cesp.existe_especie(id1) and not cesp.existe_especie(id2)) 
				cout << "ERROR: La especie " << id1 << " y la especie " << id2 << " no existen." << endl << endl;

			else cout << cesp.consultar_distancia(id1, id2) << endl << endl;
		}

		else if (opcion == "elimina_especie") {
			string id;
			cin >> id;
			cout << "# elimina_especie " << id << endl;
			if (not cesp.existe_especie(id)) cout << "ERROR: La especie " << id << " no existe." << endl;
			else cesp.eleminar_especie(id);
			cout << endl;
		}

		else if (opcion == "existe_especie") {
			string id;
			cin >> id;
			cout << "# existe_especie " << id << endl;
			if (cesp.existe_especie(id)) cout << "SI" << endl << endl;
			else cout << "NO" << endl << endl;
		}

		else if (opcion == "lee_cjt_especies") {
			cout << "# lee_cjt_especies" << endl;
			cesp.leer_especies();
			cout << endl;
		}

		else if (opcion == "imprime_cjt_especies") {
			cout << "# imprime_cjt_especies" << endl;
			cesp.imprimir_especies();
			cout << endl;
		}

		else if (opcion == "tabla_distancias") {
			cout << "# tabla_distancias" << endl;
			cesp.imprimir_distancias();
			cout << endl;
		}

		else if (opcion == "inicializa_clusters") {
			cout << "# inicializa_clusters" << endl;
			clust.inicializar(cesp);
			clust.imprimir_distancias();
			cout << endl; 
		}

		else if (opcion == "ejecuta_paso_wpgma") {
			cout << "# ejecuta_paso_wpgma" << endl;
			if (clust.num_clusters() <= 1) cout << "ERROR: num_clusters <= 1" << endl << endl;
			else {
				clust.paso_wpgma();
				clust.imprimir_distancias();
				cout << endl;
			}
		}

		else if (opcion == "ejecuta_paso_clust") {
			cout << "# ejecuta_paso_wpgma" << endl;
			if (clust.num_clusters() <= 1) cout << "ERROR: num_clusters <= 1" << endl << endl;
			else {
				clust.paso_upgma();
				clust.imprimir_distancias();
				cout << endl;
			}
		}

		else if (opcion == "imprime_cluster") {
			string id;
			cin >> id;
			cout << "# imprime_cluster " << id << endl;
			if (not clust.existe_cluster(id)) cout << "ERROR: El cluster " << id << " no existe."; 
			else clust.imprimir_cluster(id);
			cout << endl << endl;
		}

		else if (opcion == "imprime_arbol_filogenetico") {
			cout << "# imprime_arbol_filogenetico" << endl;
			clust.formar_arbol(cesp);
			if (clust.num_clusters() == 0) cout << "ERROR: El conjunto de clusters es vacio.";
			else clust.imprimir_arbol();
			cout << endl << endl;

		}
	}
}