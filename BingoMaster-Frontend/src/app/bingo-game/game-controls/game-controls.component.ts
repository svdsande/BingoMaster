import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subject, Subscription, timer } from 'rxjs';
import { repeatWhen, switchMap, takeUntil } from 'rxjs/operators';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'game-controls',
  templateUrl: './game-controls.component.html',
  styleUrls: ['./game-controls.component.scss']
})
export class GameControlsComponent implements OnInit, OnDestroy {

  @Input() drawnNumbers: number[];
  @Output() timerElapsed: EventEmitter<void> = new EventEmitter<void>();
  @Output() stopGame: EventEmitter<void> = new EventEmitter<void>();

  public playing: boolean;
  public timerDelay: number = 2000;
  private start: Subject<void> = new Subject<void>();
  private suspend: Subject<void> = new Subject<void>();
  private subscription: Subscription = new Subscription();

  constructor(private dialog: MatDialog) { }

  ngOnInit(): void {
    this.initiateTimer();
    this.suspend.next();
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
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
      if (stopGame) {
        this.stopGame.emit();
      }
    });
  }

  public rewind(): void {
    this.timerDelay += 500;
  }

  public forward(): void {
    this.timerDelay -= 500;
  }

  private initiateTimer(): void {
    this.subscription.add(
      timer(0, this.timerDelay).pipe(
        switchMap(async () => this.timerElapsed.emit()),
        takeUntil(this.suspend),
        repeatWhen(() => this.start)
      ).subscribe()
    );
  }
}
