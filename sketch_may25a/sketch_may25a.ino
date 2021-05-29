#include <Servo.h> //используем библиотеку для работы с сервоприводом

Servo servo; //объявляем переменную servo типа Servo
char inputBuffer[10];
char lockerKey[10] = {'A', 'F', 'p', 'E', 'M', 'N', 'z', 'j', 'R', 'T'};

void setup() //процедура setup
{
Serial.begin(9600);
servo.attach(10); //привязываем привод к порту 10
servo.write(90);
}

void loop() //процедура loop 
{
  if (Serial.available() > 0) {
    Serial.readBytes(inputBuffer, 10);

    for(int i = 0; i < 10; i++){
        if(inputBuffer[i] != lockerKey[i])
          break;
        if(i == 9){
            servo.write(170);
            delay(1000);
            servo.write(90);
          }
        
      }
  }
 }
