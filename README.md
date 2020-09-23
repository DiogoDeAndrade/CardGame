# Card Game Prototype 

This is a sample project that shows an extremely simple card game, using scriptable objects, and object management.

![Image](https://github.com/DiogoDeAndrade/CardGame/raw/master/Screenshots/screen01.png)

This is a card game for two players, where they take turns to get their HP down to zero.

Cards can have 3 types: energy, creature and targeted spell.
Energy is used to use the other types of cards. There isn't different types of energy, it's all the same, and there's a maximum of energy per player. In the beginning of the turn, all energy cards on the table will feed the player energy.
Creatures use energy to be summoned, and can attack the player. If the enemy player has untapped creatures (i.e. creatures that haven't attacked in the previous turn), they are used to block the player attack. Any leftover damage gets passed to the player.
Finally, the spells can target cards of certain types (at the moment, either all or just creatures) and deal damage or heal the target.

All the game rules can be customized by changing the Game Rules scriptable object.

# Improvements

There's a lot of stuff that can be done with this framework. Out of the box, without any coding, you can add more types of cards, just playing with the energy costs and attack/defense parameters, testing different decks to see how it behaves.
If you want to get your hands dirty on the code, you can extend this in multiple ways. Some suggestions:

* Attacking creatures take damage from defending creatures
* Family of spells/creatures/etc
* Spells can affect specific families of creatures
* Spells that can attack the enemy player
* Spells that can affect multiple targets
* Token system to increase the power of creatures
* Plenty of more stuff, the limit is your imagination! :)

## Licences

All code in this repo is made available through [GPLv3].

* [NaughtyAttributes] by Denis Rizov [MIT]
* Card Template by Cethiel (https://opengameart.org/content/card-template-0) [CC0]
* Imp by William.Thompsonj (https://opengameart.org/content/lpc-imp) [CC4]
* Arrow icon by Freepik (https://www.flaticon.com/authors/freepik)
* Background from NASA (http://www.nasa.com)
* Other images from [Wikipedia]

## Metadata

* Autor: [Diogo Andrade]


[GPLv3]:https://www.gnu.org/licenses/gpl-3.0.en.html
[CC BY-NC-SA 4.0]:https://creativecommons.org/licenses/by-nc-sa/4.0/
[CC0]:https://creativecommons.org/publicdomain/zero/1.0/
[CC4]:https://creativecommons.org/licenses/by/4.0/
[MIT]:https://github.com/dbrizov/NaughtyAttributes/blob/master/LICENSE
[Wikipedia]: https://www.wikipedia.org
[NaughtyAttributes]: https://github.com/dbrizov/NaughtyAttributes
[Diogo Andrade]:https://github.com/DiogoDeAndrade
