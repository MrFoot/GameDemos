using UnityEngine;
using System.Collections;
using FootStudio.Framework;

public class TestCharacter :CharacterBase<CharacterAction>  {

    public TestCharacter()
        :base()
    {
        CharacterStateManager = new TestCharacterStateManager(this);
    }

    void Start()
    {

    }
	
}
