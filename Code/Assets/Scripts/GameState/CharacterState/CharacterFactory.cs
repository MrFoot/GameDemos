using UnityEngine;
using System.Collections;

public class CharacterFactory {

    static public void Init() {



    }

    static public CharacterBase CreateCharacter(int characterId) {

        return new CharacterBase("Cube");
    }
	
}
