const int trigPin = 2;
const int echoPin = 3;
const int firePin = 5;
// defines variables
long duration;
int distance;

void setup() 
{
  pinMode(trigPin, OUTPUT); // Sets the trigPin as an Output
  pinMode(echoPin, INPUT); // Sets the echoPin as an Input
  pinMode(firePin, INPUT);
  digitalWrite(firePin,HIGH);
  
  Serial.begin(9600); // Starts the serial communication
}
void loop() 
{
  String fire = "nofire";
  if (digitalRead(firePin) == LOW)
  {
    fire = "fire";
  }
  
  // Clears the trigPin
  digitalWrite(trigPin, LOW);
  delayMicroseconds(2);
  // Sets the trigPin on HIGH state for 10 micro seconds
  digitalWrite(trigPin, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigPin, LOW);
  // Reads the echoPin, returns the sound wave travel time in microseconds
  duration = pulseIn(echoPin, HIGH,10000);  
  
  Serial.println(fire + "|" + duration);  
  
  //delayMicroseconds(1000);
}
