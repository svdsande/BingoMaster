import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { take } from 'rxjs/operators';
import { BingoGameModel, PlayerModel } from 'src/api/api';
import { BingoGameService } from '../services/bingo-game.service';
import { WinnerDialogComponent } from './winner-dialog/winner-dialog.component';

@Component({
  selector: 'app-bingo-game',
  templateUrl: './bingo-game.component.html',
  styleUrls: ['./bingo-game.component.scss']
})
export class BingoGameComponent implements OnInit {

  public drawnNumbers: number[] = [];
  public bingoGame: BingoGameModel;
  public countDownDone: boolean = false;

  constructor(
    private bingoGameService: BingoGameService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.bingoGameService.nextRoundReceived
      .pipe(take(1))
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
