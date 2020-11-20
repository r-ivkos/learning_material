OPCIONS = -D_JUDGE_ -D_GLIBCXX_DEBUG -O2 -Wall -Wextra -Werror -Wno-sign-compare -std=c++11

program.exe: program.o Especie.o cjt_especies.o Cluster.o cjt_clusters.o
	g++ -o program.exe program.o Especie.o cjt_especies.o Cluster.o cjt_clusters.o
	rm *.o

program.o: program.cc 
	g++ -c program.cc $(OPCIONS)

Especie.o: Especie.cc Especie.hh
	g++ -c Especie.cc $(OPCIONS)

cjt_especies.o: cjt_especies.hh cjt_especies.cc
	g++ -c cjt_especies.cc $(OPCIONS)

Cluster.o: Cluster.cc Cluster.hh
	g++ -c Cluster.cc $(OPCIONS)

cjt_clusters.o: cjt_clusters.hh cjt_clusters.cc
	g++ -c cjt_clusters.cc $(OPCIONS)

	
clean:
	rm *.o
	rm *.exe
	rm *.gch