import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { BingoGameModel, PlayerModel } from 'src/api/api';
import { BingoGameService } from 'src/app/services/bingo-game.service';
import { WinnerDialogComponent } from './winner-dialog/winner-dialog.component';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.scss']
})
export class GameComponent implements OnInit {

  public drawnNumbers: number[] = [];
  public bingoGame: BingoGameModel = new BingoGameModel();
  public countDownDone: boolean = false;

  constructor(
    private bingoGameService: BingoGameService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    //TODO: Fetch game from backend (send request to API)

    this.bingoGameService.nextRoundReceived
      .subscribe((model: BingoGameModel) => {
        if (this.gameIsWon(model.players)) {
          const gameWinner = model.players.find(player => player.isFullCardDone);
          this.gameWon(gameWinner);
        } else {
          this.drawnNumbers.push(model.drawnNumber);
          this.bingoGame.players = model.players;
        }
      });
  }

  public onCountDownDone(): void {
    this.countDownDone = true;
    this.drawnNumbers.push(this.bingoGame.drawnNumber);
  }

  public playNextRound(): void {
    this.bingoGameService.playNextRound(this.bingoGame.players, this.drawnNumbers);
  }

  public stopGame(): void {
    this.bingoGame = undefined;
    this.drawnNumbers = [];
  }

  private gameWon(player: PlayerModel): void {
    const dialogRef = this.dialog.open(WinnerDialogComponent, {
      width: '500px',
      data: { playerName: player.name }
    });

    dialogRef.afterClosed().subscribe(() => {
      this.stopGame();
    });
  }

  private gameIsWon(players: PlayerModel[]): boolean {
    return players.filter(player => player.isFullCardDone).length > 0;
  }
}
