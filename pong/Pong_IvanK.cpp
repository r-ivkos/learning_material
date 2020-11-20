#include <iostream>
#include <winbgim.h>
#include <time.h>
#include <math.h>
#include <windows.h>
using namespace std;

const int WindowX = 900;
const int WindowY = 500;

void StartWindow(); //Pantalla inicial
void GameOver(int points);
void VictoryWindow();
void drawBall(int x, int y, int rad, int color); //dibuixar la pilota
void drawBar(int x, int y, int width, int height, int color); //dibuixar la pala
void showInfo(int points, int lives); //mostrar els punts i vides que porta el jugador

int main()
{
	
	StartWindow(); 
	system("exit");
	srand(time(NULL));
	int color_orange = COLOR(249, 166, 2);
	int color_aquamarine = COLOR(68, 201, 154);
	int GameWindowX = 800;
	int GameWindowY = 600;		
    initwindow(GameWindowX, GameWindowY);
    delay(100);
    
    bool victory = false;
    int lives = 3;
    int points = 0;
    
	int ball_x = getmaxx()/2, ball_y = 50, ball_rad = 10;
	int ball_speed = 40;
	int ball_acceleration = 2;
	int ball_x_incr = 2+rand()%8, ball_y_incr = 2+rand()%8;
	
	int bar_width = 140, bar_height = 20;
	int bar_x = getmaxx()/2-bar_width/2, bar_y = getmaxy()-50;
	int bar_speed = 30;
	
	int in_bar_x = bar_x, in_bar_y = bar_y, in_ball_x = ball_x, in_ball_y = ball_y; // guardar les posicions inicials pel reinici del joc
	char key;
		
	while(true){
		drawBall(ball_x, ball_y, ball_rad, color_orange);
		drawBar(bar_x, bar_y, bar_width, bar_height, color_aquamarine);
		showInfo(points, lives);
		//delay(ball_speed);
		Beep(0, ball_speed);
		drawBall(ball_x, ball_y, ball_rad, 0);
		drawBar(bar_x, bar_y, bar_width, bar_height, 0);	
		if(ball_x+ball_rad>=getmaxx() || ball_x-ball_rad<=0) ball_x_incr = -ball_x_incr; //colisio amb els costats de la finestra
		if(ball_y-ball_rad<=0) ball_y_incr = -ball_y_incr;
		
		if (ball_y+ball_rad >= getmaxy()) {  //si la pilota toca la part inferior de la pantalla es perd 1 vida i es reinicia el joc
			lives--;
			if (lives <= 0){
				break; //si s'han perdut totes les vides surt del bucle (finalitza el joc)
			}
			cleardevice();
			showInfo(points, lives);
			ball_x = in_ball_x, ball_y = in_ball_y;
			bar_x = in_bar_x, bar_y = in_bar_y;            
			drawBall(ball_x, ball_y, ball_rad, color_orange);
			ball_x_incr = 2+rand()%8, ball_y_incr = 2+rand()%8;
		    drawBar(bar_x, bar_y, bar_width, bar_height, color_aquamarine);
			Beep(0, 1000);
			drawBall(ball_x, ball_y, ball_rad, 0);
		    drawBar(bar_x, bar_y, bar_width, bar_height, 0);
			
		}
		
		if ((ball_y+ball_rad+5 >= bar_y) && (ball_x + ball_rad >= bar_x && ball_x - ball_rad <= bar_x+bar_width)){ //colisio amb la pala
			ball_y_incr = -ball_y_incr;
			points += sqrt(pow(abs(ball_x_incr),2) + pow(abs(ball_y_incr), 2)); //la quantitat de punts que suma cada vega que la pilota rebota amb la pala,
			                                                                    // es igual a la velocitat que porta la pilota en aquest momet
			                                                                    
			ball_y_incr -= ball_acceleration;  //acclerar la pilota cada vega que rebota amb la pala
			if (ball_x_incr < 0) ball_x_incr -= ball_acceleration;
			else ball_x_incr += ball_acceleration;
			
			if (points >= 1000){
				victory = true; //si el jugador ha conseguit 1000 punts, guanya
				break; //el programa surt del bucle (el joc finalitza)
			}	
		} 
		
		
		if (kbhit()){ //moure la pala des del teclat
			key = getch();
			if (key == 'a' && bar_x > 5){
				bar_x -= bar_speed;  
			}
			if (key == 'd' && (bar_x + bar_width) < getmaxx()-5){
				bar_x += bar_speed;
			}
		}
		ball_x += ball_x_incr;
		ball_y += ball_y_incr;
	}
	
	
	closegraph();
	if (!victory) GameOver(points); 
	else VictoryWindow();
	

	
	
	
	
}



void drawBar(int x, int y, int width, int height, int color){
	setfillstyle(SOLID_FILL, color);
	setcolor(color);
	bar(x, y, x+width, y+height);
}

void drawBall(int x, int y, int rad, int color){
	setfillstyle(SOLID_FILL, color);
	setcolor(color);
	fillellipse(x, y, rad, rad);
	
}

void showInfo(int points, int lives){
	char msg[80];
	settextjustify(LEFT_TEXT, CENTER_TEXT);
	settextstyle(2, HORIZ_DIR, 7);
	setcolor(0);
	outtextxy(25, 40, "a                            a"); //borrar el text que mostra els punts
	setcolor(15);
	sprintf(msg, "PUNTOS:   %d", points);
	outtextxy(25, 40, msg);
	sprintf(msg, "VIDAS:   %d", lives);
	settextstyle(2, HORIZ_DIR, 7);
	settextjustify(RIGHT_TEXT, CENTER_TEXT);
	outtextxy(getmaxx()-25, 40, msg);
	
}


void GameOver(int points){
	char msg[80];
	initwindow(WindowX, WindowY);
	setcolor(4);
	settextjustify(CENTER_TEXT, CENTER_TEXT);
	settextstyle(0, VERT_DIR, 3);
	outtextxy(getmaxx()/2, 160, "GAME OVER");
	settextstyle(3, VERT_DIR, 3);
	setcolor (1);
	sprintf(msg, "Has conseguido %d puntos", points);
	outtextxy(getmaxx()/2, getmaxy()/2-50, msg);
	setcolor(8);
	outtextxy(getmaxx()/2, getmaxy()/2 + 30, "Pulsa ENTER para continuar");
	char key;
	while (key != int(13))
	{
		key = getch();	
	}
}

void VictoryWindow(){
	initwindow(WindowX, WindowY);
	setcolor(3);
	settextjustify(CENTER_TEXT, CENTER_TEXT);
	settextstyle(0, VERT_DIR, 3);
	outtextxy(getmaxx()/2, 160, "FELICIDADES");
	outtextxy(getmaxx()/2, getmaxy()/2-50, "HAS GANADO!");
	setcolor(8);
	settextstyle(3, VERT_DIR, 3);
	outtextxy(getmaxx()/2, getmaxy()/2 + 30, "Pulsa ENTER para continuar");
	char key;
	while (key != int(13))
	{
		key = getch();	
	}
	
}

void StartWindow(){
 	initwindow(WindowX, WindowY);
	setcolor(4);
	settextjustify(CENTER_TEXT, CENTER_TEXT);
	settextstyle(0, VERT_DIR, 3);
	outtextxy(getmaxx()/2, 100, "PONG");
	settextstyle(3, VERT_DIR, 2);
	setcolor(3);
	outtextxy(getmaxx()/2, getmaxy()/2 -95, "A/D - mover la pala");
	outtextxy(getmaxx()/2, getmaxy()/2 -65, "Para ganar consigue 1000 puntos");
	settextstyle(3, VERT_DIR, 3);
	setcolor(10);
	outtextxy(getmaxx()/2, getmaxy()/2 + 30, "Pulsa ENTER para empezar");
	settextstyle(3, HORIZ_DIR, 2);
	settextjustify(LEFT_TEXT, LEFT_TEXT);
	setcolor (8);
	outtextxy(getmaxx()/2+150, getmaxy()/2 + 120, "by Ivan Kosovtsev");
	char key;
	while (key != int(13)){
		key = getch();
	}
	closegraph();
}
