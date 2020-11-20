

class button{ //clase botón de texto
  float x; //posion del botón
  float y;
  String text; //el texto del botón
  boolean selected; //si el raton esta encima del botón
  float textWidth; //anchura del botón
  float textHeight; //altura del botón
  color col; //color del texto
  color sel; //color del texto seleccionado
  
  public button(float x, float y, String text, color col, color sel){ //constructor con parámetros necesarios al crear un botón
    this.text = text;
    this.x = x;
    this.y = y;
    textWidth = textWidth(text);
    textHeight = textAscent();
    this.col = col;
    this.sel = sel;
  }
  void update(){ //actualizar el estado del botón
    textAlign(CENTER, CENTER);
    if(mouseX >= x-textWidth/2 && mouseX <= x+textWidth/2 &&
      mouseY >= y-textHeight/2 && mouseY <= y+textHeight/2){ //si el raton está encima
      selected = true; 
      fill(sel); //se cambia el texto a color del texto seleccionado
      }
    else{ 
      selected = false;
      fill(col); //si no esta selecionado tiene el color normal
    }
    text(text, x, y); //dibuja el texto
  }
}
