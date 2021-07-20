using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGo = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGo.GetComponent<Unit>();

        GameObject enemyGo = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGo.GetComponent<Unit>();

        dialogueText.text = "Se acerca un " + enemyUnit.unitNombre + " Salvaje...";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        // Damage al enemigo

        bool isDead = enemyUnit.TakeDamage(playerUnit.Damage);

        enemyHUD.SetHP(enemyUnit.CurrentHP);
        dialogueText.text = "El Ataque fue perfecto!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            // fin de la pelea
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            // turno enemigo
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitNombre + " golpea!";

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.Damage);

        playerHUD.SetHP(playerUnit.CurrentHP);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "Ganaste la pelea!";
            EditorSceneManager.LoadScene("Victoria");
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "Has perdido.";
            EditorSceneManager.LoadScene("Derrota");
        }
    }

    void PlayerTurn()
    {
        dialogueText.text = "Elije una accion:";
    }

    private IEnumerator PlayerHeal()
    {
        playerUnit.Heal(15);

        playerHUD.SetHP(playerUnit.CurrentHP);
        dialogueText.text = "Recuperas energias!";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerHeal());
    }
}
