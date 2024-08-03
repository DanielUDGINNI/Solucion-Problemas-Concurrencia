# Solucion-Problemas-Concurrencia
Estos programas simulan la aplicación de soluciones a problemas de concurrencia en memoria utilizando distintos algoritmos.

## Filósofos comensales
<p>Cinco filósofos viven en una casa, donde hay una mesa preparada para ellos. Básicamente, la vida de cada
filósofo consiste en pensar y comer, y después de años de haber estado pensando, todos los filósofos están
de acuerdo en que la única comida que contribuye a su fuerza mental son los espaguetis. Debido a su falta
de habilidad manual, cada filósofo necesita dos tenedores para comer los espaguetis.</p>
<p>El problema: diseñar un ritual (algoritmo) que permita a los
filósofos comer. El algoritmo debe satisfacer la exclusión mutua (no puede haber dos filósofos que
puedan utilizar el mismo tenedor a la vez) evitando el interbloqueo y la inanición (en este caso, el término
tiene un sentido literal, además de algorítmico).</p>

### Solución
El funcionamiento se basa en elegir un filósofo para comer y liberarlo en algún momento dado para que
otro pueda comer. Siempre se tiene que poder liberar para que los demás puedan comer y asignar bien las
variables correspondientes.
<p>Propuesta
  <ul>
    <li>Lista enlazada circular</li>
    <li>Clase filósofo</li>
    <li>Método liberar tenedores</li>
    <li>Método obtener tenedores</li>
    <li>Timers</li>
    <li>Variables booleanas para el manejo de la disponibilidad de tenedores</li>
  </ul>
</p>
<img src="images/filosofos1.PNG" width="500" height="400" />
<img src="images/filosofos2.PNG" width="500" height="400" />
<img src="images/filosofos3.PNG" width="500" height="400" />
<p>Fuente: Stallings, W. (2005). Sistemas Operativos (5ta ed.). Pearson.</p>

## Semáforos
<p>Semáforo: El principio fundamental es éste: dos o más procesos pueden cooperar por medio de simples
señales, tales que un proceso pueda ser obligado a parar en un lugar específico hasta que haya recibido
una señal específica. Cualquier requisito complejo de coordinación puede ser satisfecho con la estructura
de señales apropiada. Para la señalización, se utilizan unas variables especiales llamadas semáforos. Para
transmitir una señal vía el semáforo s, el proceso ejecutará la primitiva semSignal(s). Para recibir una 
señal vía el semáforo s, el proceso ejecutará la primitiva semWait(s); si la correspondiente señal no se ha
transmitido todavía, el proceso se suspenderá hasta que la transmisión tenga lugar.</p>

## Solución
<p> Para conseguir el efecto deseado, el semáforo puede ser visto como una variable que contiene un valor entero sobre el cual sólo están definidas tres operaciones: </p>
<p>Un semáforo puede ser inicializado a un valor no negativo. La operación semWait decrementa el valor del semáforo. Si el valor pasa a ser negativo, entonces el
proceso que está ejecutando semWait se bloquea. En otro caso, el proceso continúa su ejecución. La operación semSignal incrementa el valor del semáforo. Si el valor es menor o igual que cero, entonces
se desbloquea uno de los procesos bloqueados en la operación semWait. Aparte de estas tres operaciones no hay manera de inspeccionar o manipular un semáforo.</p>
<img src="images/Semaforos1.PNG" width="500" height="400" />
<img src="images/Semaforos2.PNG" width="500" height="400" />

<p>Fuentes:</p>
<p>Stallings, W. (2005). Sistemas Operativos (5ta ed.). Pearson.</p>
<p>Vega, H. (2023). Problemas de la concurrencia./Clases/2211-historia-programacion/35081-problemas-de-la-concurrencia/.
  https://platzi.com/clases/2211-historia-programacion/35081-problemas-de-la-concurrencia/</p>
<p>Tanenbaum, A. S. (2009). SISTEMAS OPERATIVOS MODERNOS (3a. ed.).PEARSON EDUCACIÓN,
México,</p>

## Banquero
<p>El Algoritmo del banquero, en sistemas operativos es una forma de evitar el interbloqueo, propuesta por
primera vez por Edsger Dijkstra. Es un acercamiento teórico para evitar los interbloqueos en la
planificación de recursos. Requiere conocer con anticipación los recursos que serán utilizados por todos
los procesos. Esto último generalmente no puede ser satisfecho en la práctica. Este algoritmo usualmente
es explicado usando la analogía con el funcionamiento de un banco. Los clientes representan a los
procesos, que tienen un crédito límite, y el dinero representa a los recursos.</p><p>El banquero es el sistema
operativo. El banco confía en que no tendrá que permitir a todos sus clientes la utilización de todo su
crédito a la vez. El banco también asume que si un cliente maximiza su crédito será capaz de terminar sus
negocios y retornar el dinero de vuelta a la entidad, permitiendo servir a otros clientes. </p> <p>El algoritmo
mantiene al sistema en un estado seguro. Un sistema se encuentra en un estado seguro si existe un orden
en que pueden concederse las peticiones de recursos a todos los procesos, previniendo el interbloqueo. El
algoritmo del banquero funciona encontrando estados de este tipo. Los procesos piden recursos, y son
complacidos siempre y cuando el sistema se mantenga en un estado seguro después de la concesión. De lo
contrario, el proceso es suspendido hasta que otro proceso libere recursos suficientes.</p>

## Solución
<p>La solución y propuesta del algoritmo del banquero la estoy basando en un estado seguro del banco en
donde tiene un tamaño fijo de recursos y un límite de entrega de recursos, donde si los recursos (dinero)
que piden los procesos (clientes) rebasa y dejaría en un estado inseguro entonces no se puede otorgar
dinero. Los clientes regresan cierta cantidad de dinero de forma aleatoria, liberando recursos para que
otros clientes puedan seguir pidiendo y que el dinero circule. Se tiene variable booleana para indicar el
estado del banco, una lista con cuentas de los clientes iniciadas en 0. El timer va a controlar cada petición
de los procesos como obtener o regresar dinero.</p>

<img src="images/banco01.PNG" width="500" height="400" />
<img src="images/banco02.PNG" width="500" height="400" />

<p>Fuente</p>
Reyes Brañez, Mario. Algoritmo del banquero : aplicado al sistema visado de poderes caso BBVA Banco
Continental. (2011). Universidad Mayor de San Marcos.
https://cybertesis.unmsm.edu.pe/bitstream/handle/20.500.12672/13878/Branez_Reyes_Marlon_2011.pdf?sequence=1&isAllowed=y



