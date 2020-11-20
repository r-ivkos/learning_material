

PImage lab; //imagen del laberinto
int ball_x = 315, ball_y = 585; //posicion inicial de la bola
int incr = 5; //velocidad de la bola
int dir = 0; //direccion de la bola (0 - abajo, 1 - arriba, 2 - derecha, 3 - izquierda) 
rectangles rect[][] = new rectangles[20][20]; //dibuja rectangulos invisibles que corresponden a una celula del laberinto
color the_way = color(255, 255, 255, 1);//color que tienen las celulas del camino hacia la salida
color dead_end = color(0, 255, 0, 100);  //color que tienen las celulas del camino hacia una pared (opacidad 1, por tanto el usuario no los ve)
//para ver mejor el funcionamiento de las celulas, cambia a dead_end = color(0, 255, 0, 100);
void setup() {
  size (600, 600); //tamaño de la ventana
  int selector = int(random(1, 11)); // selector aleatorio de laberinto (hay 10)
  lab = loadImage("labs\\lab"+selector+".png"); //asignar a la variable la imagen aleatoria del laberinto
  noStroke();
  rectMode(CENTER);
  for (int i = 0; i < 20; i++) {
    for (int y = 0; y < 20; y++) {
      rect[i][y] = new rectangles(17+y*29.8, 17+i*29.8, the_way); //crear las celulas
    }
  }
}

void draw() {
  image(lab, 0, 0, width, height); // dibuja el laberinto
  for (int i = 0; i < 20; i++) {
    for (int y = 0; y < 20; y++) { //bucle que se repite para actualizar informacion de cada celula
      if (rect[i][y].c != dead_end) { //si la celula no es de color que lleva al callejon sin salida
        for (int n = 0; n <20; n++) { //este bucle en concreto sirve para ayudar a comprobar el color no solo de 1 pixel, si no de una serie de pixels, para asegurar que hay o no pixel de color negro
          //si la celula esta en una esquina (de las 4 posibles), se pinta. 
          //es decir, si la celula esta rodeada esta rodeada con 3 paredes negras, se pinta.
          if (get(int(rect[i][y].x) + 13 + (n-10), int(rect[i][y].y)) <= color(50)&&
            get(int(rect[i][y].x) - 13 - (n-10), int(rect[i][y].y)) <= color(50)&&
            get(int(rect[i][y].x), int(rect[i][y].y) + 13 + (n-10)) <= color(50)) { 
            rect[i][y].c = dead_end;
          } else if (get(int(rect[i][y].x) + 13 + (n-10), int(rect[i][y].y)) <= color(50)&&
            get(int(rect[i][y].x) - 13 - (n-10), int(rect[i][y].y)) <= color(50)&&
            get(int(rect[i][y].x), int(rect[i][y].y) - 13 - (n-10)) <= color(50)) {
            rect[i][y].c = dead_end;
          } else if (get(int(rect[i][y].x) + 13 + (n-10), int(rect[i][y].y)) <= color(50)&&
            get(int(rect[i][y].x), int(rect[i][y].y) + 13 + (n-10)) <= color(50)&&
            get(int(rect[i][y].x), int(rect[i][y].y) - 13 - (n-10)) <= color(50)) {
            rect[i][y].c = dead_end;
          } else if (get(int(rect[i][y].x) - 13 - (n-10), int(rect[i][y].y)) <= color(50)&&
            get(int(rect[i][y].x), int(rect[i][y].y) + 13 + (n-10)) <= color(50)&&
            get(int(rect[i][y].x), int(rect[i][y].y) - 13 - (n-10)) <= color(50)) {
            rect[i][y].c = dead_end;
          }
         
          else { // si la celula no esta en ninguna esquina, se comprueba si la de al lado ya esta pintada y si la celula esta limitada por 2 paredes
                // y ademas si esta celula con la de al lado no estan separadas con una pared (asi se pintan los caminos evidentes hasta llegar a un cruce)
            if (i != 0) {  
              if ((rect[i-1][y].c == dead_end && get(int(rect[i][y].x), int(rect[i][y].y) - 13 - (n-10)) > color(150)) && 
                ((get(int(rect[i][y].x) + 13 + (n-10), int(rect[i][y].y)) <= color(50) && get(int(rect[i][y].x) - 13 - (n-10), int(rect[i][y].y)) <= color(50))||
                (get(int(rect[i][y].x) - 13 - (n-10), int(rect[i][y].y)) <= color(50) && get(int(rect[i][y].x), int(rect[i][y].y) + 13 + (n-10)) <= color(50))||
                (get(int(rect[i][y].x) + 13 + (n-10), int(rect[i][y].y)) <= color(50) && get(int(rect[i][y].x), int(rect[i][y].y) + 13 + (n-10)) <= color(50))))
              {
                rect[i][y].c = dead_end;
              }
            }
            if (i != 19) {
              if ((rect[i+1][y].c == dead_end && get(int(rect[i][y].x), int(rect[i][y].y) + 13 + (n-10)) > color(150)) && 
                ((get(int(rect[i][y].x) + 13 + (n-10), int(rect[i][y].y)) <= color(50) && get(int(rect[i][y].x) - 13 - (n-10), int(rect[i][y].y)) <= color(50))||
                (get(int(rect[i][y].x) - 13 - (n-10), int(rect[i][y].y)) <= color(50) && get(int(rect[i][y].x), int(rect[i][y].y) - 13 - (n-10)) <= color(50))||
                (get(int(rect[i][y].x) + 13 + (n-10), int(rect[i][y].y)) <= color(50) && get(int(rect[i][y].x), int(rect[i][y].y) - 13 - (n-10)) <= color(50))))
              {
                rect[i][y].c = dead_end;
              }
            }
            if (y != 0) {
              if ((rect[i][y-1].c == dead_end && get(int(rect[i][y].x) - 13 - (n-10), int(rect[i][y].y)) > color(150)) && 
                ((get(int(rect[i][y].x), int(rect[i][y].y) + 13 + (n-10)) <= color(50) && get(int(rect[i][y].x), int(rect[i][y].y) - 13 - (n-10)) <= color(50))||
                (get(int(rect[i][y].x) + 13 + (n-10), int(rect[i][y].y)) <= color(50) && get(int(rect[i][y].x), int(rect[i][y].y) - 13 - (n-10)) <= color(50))||
                (get(int(rect[i][y].x) + 13 + (n-10), int(rect[i][y].y)) <= color(50) && get(int(rect[i][y].x), int(rect[i][y].y) + 13 + (n-10)) <= color(50))))
              {
                rect[i][y].c = dead_end;
              }
            }
            if (y != 19) {
              if ((rect[i][y+1].c == dead_end && get(int(rect[i][y].x) + 13 + (n-10), int(rect[i][y].y)) > color(150)) && 
                ((get(int(rect[i][y].x), int(rect[i][y].y) + 13 + (n-10)) <= color(50) && get(int(rect[i][y].x), int(rect[i][y].y) - 13 - (n-10)) <= color(50))||
                (get(int(rect[i][y].x) - 13 - (n-10), int(rect[i][y].y)) <= color(50) && get(int(rect[i][y].x), int(rect[i][y].y) - 13 - (n-10)) <= color(50))||
                (get(int(rect[i][y].x) - 13 - (n-10), int(rect[i][y].y)) <= color(50) && get(int(rect[i][y].x), int(rect[i][y].y) + 13 + (n-10)) <= color(50))))
              {
                rect[i][y].c = dead_end;
              }
            }

            //Para llenar los cruces, primero se guarda informacion sobre la celula, con que paredes contacta
            
            if (get(int(rect[i][y].x) + 13 + (n-10), int (rect[i][y].y)) <= color(50)) {
              rect[i][y].right_black = true;
            }
            if (get(int(rect[i][y].x) - 13 - (n-10), int (rect[i][y].y)) <= color(50)) {
              rect[i][y].left_black = true;
            }

            if (get(int(rect[i][y].x), int (rect[i][y].y) + 13 + (n-10)) <= color(50)) {
              rect[i][y].down_black = true;
            } 
            if (get(int(rect[i][y].x), int (rect[i][y].y) - 13 - (n-10)) <= color(50)) {
              rect[i][y].up_black = true;
            }
          }
        }
      }
      //para pintar los cruces se comprueba que la celula del mismo contacta con almenos 2 celulas ya pintadas y que no estan separadas con pared
      //y ademas que ese cruce solo tenga 3 direcciones, en el caso contrario, si tiene 4 direcciones, si 3 de las celulas vecinas estan pintadas, se pinta el cruce
      if (y != 0 && y != 19) {
        if (rect[i][y+1].c == dead_end && rect[i][y-1].c == dead_end&& !rect[i][y].left_black && !rect[i][y].right_black &&
          (rect[i][y].up_black || rect[i][y].down_black)) {
          rect[i][y].c = dead_end;
        }
      }
      if (i != 0 && i != 19) {
        if (rect[i+1][y].c == dead_end && rect[i-1][y].c == dead_end&& !rect[i][y].up_black && !rect[i][y].down_black &&
          (rect[i][y].left_black || rect[i][y].right_black)) {
          rect[i][y].c = dead_end;
        }
      }

      if (i != 0 && y != 0) {
        if (rect[i-1][y].c == dead_end && rect[i][y-1].c == dead_end && !rect[i][y].up_black && !rect[i][y].left_black &&
          (rect[i][y].down_black || rect[i][y].right_black)) {
          rect[i][y].c = dead_end;
        }
      }
      if (i != 0 && y != 19) {
        if (rect[i-1][y].c == dead_end && rect[i][y+1].c == dead_end && !rect[i][y].up_black && !rect[i][y].right_black && 
          ( (rect[i][y].down_black || rect[i][y].left_black))) {
          rect[i][y].c = dead_end;
        }
      }
      if (i != 19 && y != 0) {
        if (rect[i+1][y].c == dead_end && rect[i][y-1].c == dead_end && !rect[i][y].down_black && !rect[i][y].left_black && 
          (rect[i][y].up_black || rect[i][y].right_black)) {
          rect[i][y].c = dead_end;
        }
      }
      if (i != 19 && y != 19) {
        if (rect[i+1][y].c == dead_end && rect[i][y+1].c == dead_end && !rect[i][y].down_black && !rect[i][y].right_black && 
          (rect[i][y].up_black || rect[i][y].left_black)) {
          rect[i][y].c = dead_end;
        }
      }

      if (i !=0 && y != 19 && y !=0) {
        if (rect[i-1][y].c == dead_end && rect[i][y+1].c == dead_end  && rect[i][y-1].c == dead_end &&
          !rect[i][y].down_black && !rect[i][y].right_black && !rect[i][y].up_black && !rect[i][y].left_black) {
          rect[i][y].c = dead_end;
        }
      }
      if (i !=19 && y != 19 && y !=0) {
        if (rect[i+1][y].c == dead_end && rect[i][y+1].c == dead_end  && rect[i][y-1].c == dead_end &&
          !rect[i][y].down_black && !rect[i][y].right_black && !rect[i][y].up_black && !rect[i][y].left_black) {
          rect[i][y].c = dead_end;
        }
      }
      if (y !=0 && i != 19 && i !=0) {
        if (rect[i-1][y].c == dead_end && rect[i+1][y].c == dead_end  && rect[i][y-1].c == dead_end &&
          !rect[i][y].down_black && !rect[i][y].right_black && !rect[i][y].up_black && !rect[i][y].left_black) {
          rect[i][y].c = dead_end;
        }
      }
      if (y !=19 && i != 19 && i !=0) {
        if (rect[i-1][y].c == dead_end && rect[i+1][y].c == dead_end  && rect[i][y+1].c == dead_end &&
          !rect[i][y].down_black && !rect[i][y].right_black && !rect[i][y].up_black && !rect[i][y].left_black) {
          rect[i][y].c = dead_end;
        }
      }

      rect[i][y].drawRect(); //dibuja las celulas
      rect[i][y].checkBall(ball_x, ball_y); //comprueba si la celula tiene la bola dentro
      if (rect[i][y].ball_in) {
        if (i != 19) {
          //mira la siguiente celula blanca, en la que no estuvo la bola aun y la que no esta separada con pared y le asigna la direccion a la bola
          if (rect[i+1][y].c == the_way && !rect[i+1][y].ball_was_in && !rect[i][y].down_black) { 
            dir = 0; //down
          }
        }
        if (i != 0) {
          if (rect[i-1][y].c == the_way && !rect[i-1][y].ball_was_in && !rect[i][y].up_black) {
            dir = 1; //up
          }
        }
        if (y != 19) {
          if (rect[i][y+1].c == the_way && !rect[i][y+1].ball_was_in && !rect[i][y].right_black) {
            dir = 2; //right
          }
        }
        if (y != 0) {
          if (rect[i][y-1].c == the_way && !rect[i][y-1].ball_was_in && !rect[i][y].left_black) {
            dir = 3; //left
          }
        }
      }
    }
  }

  if (rect[0][9].ball_was_in) { //si la bola esta en la ultima celula, la de salida, se para
    dir = -1;
  }

  fill(255, 0, 0);
  ellipse(ball_x, ball_y, 16, 16); //dibuja la bola
  if (frameCount >= 45){ //espera un tiempo entre 1/2 y 1 segundo antes de moverse. Es necesario para dejarle al programa cargar la solucion
    switch(dir) { //dependiendo de la direccion, incrementa su posicion
    case 0: 
      ball_y+=incr; 
      break;
    case 1: 
      ball_y-=incr; 
      break;
    case 2: 
      ball_x+=incr; 
      break;
    case 3: 
      ball_x-=incr; 
      break;
    }
  }
}

class rectangles { //la clase usada para las celulas
  float x; //posicion de la celula
  float y;
  color c; // color de la celula
  boolean up_black, left_black, right_black, down_black, ball_in, ball_was_in;
  public rectangles(float x, float y, color c) {
    this.x = x;
    this.y = y;
    this.c = c;
  }

  void drawRect() {
    fill(c);
    rect(x, y, 26, 26);
  }

  void checkBall(int x, int y) {
    if (x >= this.x-2 && x <= this.x+2 && y >= this.y-2 && y <= this.y+2) { //comprueba si la bola esta dentro de la celula
      ball_in = true;
      ball_was_in = true;
    } else ball_in = false;
  }
}
