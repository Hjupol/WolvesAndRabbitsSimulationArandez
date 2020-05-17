En esta carpeta encontrarán el código de una pequeña aplicación que simula el comportamiento de un grupo de conejos de forma simplificada.

Existen 2 tipos de entidades: Rabbit, que representa a los conejos, y Grass, que representa al pasto que los conejos pueden comer. Los conejos se grafican con un cuadrado de color blanco y el pasto se grafica con un cuadrado cuyo color depende de cuán crecido esté el mismo.

En cada frame, los conejos se moverán al azar por el escenario intentando comer el pasto que encuentren en sus inmediaciones y reproducirse con cualquier conejo que encuentren cerca. La reproducción sólo será posible si el conejo está en edad reproductiva y tiene suficiente comida. Cada conejo puede dar hasta 30 crías por frame. Si la cantidad de comida del conejo se acaba o si el conejo sobrevive luego de una cierta cantidad de frames, las chances de morir del mismo se incrementan. La simulación se reinicia si la cantidad de conejos vivos en algún momento llega a cero.

Esta implementación está construida de forma intencionalmente ineficiente. Pueden modificar cualquier parte del código siempre y cuando mantengan la misma funcionalidad.

Consignas
1. Realizar mediciones de performance que permitan diagnosticar el problema.
2. Escribir qué problemas encontró en la simulación en función de las mediciones.
3. Proponer una solución al problema encontrado, describirla en sus palabras con el mayor detalle posible.
4. Implementar la solución planteada haciendo los cambios en el código que sean necesarios.
5. Validar la implementación realizando nuevas mediciones. ¿Se resolvió el problema? En caso negativo, ¿qué otras soluciones alternativas se le ocurren?

PUNTOS EXTRA: Una vez resuelto el problema de optimización, incorporar una tercer entidad a la simulación: los lobos (Wolf, en inglés). Los lobos deberían deambular por el escenario buscando conejos para comer.