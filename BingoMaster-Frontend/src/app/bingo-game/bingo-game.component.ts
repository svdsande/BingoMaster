import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subject, Subscription, timer } from 'rxjs';
import { repeatWhen, switchMap, take, takeUntil } from 'rxjs/operators';
import { BingoGameModel } from 'src/api/api';
import { BingoGameService } from '../services/bingo-game.service';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-bingo-game',
  templateUrl: './bingo-game.component.html',
  styleUrls: ['./bingo-game.component.scss']
})
export class BingoGameComponent implements OnInit {

  public drawnNumbers: number[] = [];
  public bingoGame: BingoGameModel;
  public countDownDone: boolean = false;
  public playing: boolean;
  public timerDelay: number = 2000;
  private start: Subject<void> = new Subject<void>();
  private suspend: Subject<void> = new Subject<void>();
  private subscription: Subscription = new Subscription();

  constructor(
    private bingoGameService: BingoGameService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.bingoGameService.nextRoundReceived
      .pipe(take(1))
      .subscribe((model: BingoGameModel) => {
        this.drawnNumbers.push(model.drawnNumber);
        this.bingoGame.players = model.players;
      });

    this.initiateTimer();
    this.suspend.next();
  }

  public play(): void {
    this.start.next();
    this.playing = true;
  }

  public pause(): void {
    this.suspend.next();
    this.playing = false;
  }

  public stop(): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '500px',
    });

    dialogRef.afterClosed().subscribe(stopGame => {
      console.log('The dialog was closed', stopGame);
    });
  }

  //TODO: Maybe switchMap so that timer could be restarted or updated
  public rewind(): void {
    this.timerDelay += 500;
  }

  public forward(): void {
    this.timerDelay -= 500;
  }

  public onCountDownDone(): void {
    this.countDownDone = true;
    this.drawnNumbers.push(this.bingoGame.drawnNumber);
  }

  private initiateTimer(): void {
    this.subscription.add(
      timer(0, this.timerDelay).pipe(
        switchMap(async () => this.playNextRound()),
        takeUntil(this.suspend),
        repeatWhen(() => this.start)
      ).subscribe()
    );
  }

  private playNextRound(): void {
    this.bingoGameService.playNextRound(this.bingoGame.players, this.drawnNumbers);
  }
}
