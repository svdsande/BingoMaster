import { animate, style, transition, trigger } from '@angular/animations';
import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subject, Subscription, timer } from 'rxjs';
import { repeatWhen, switchMap, takeUntil } from 'rxjs/operators';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'game-controls',
  templateUrl: './game-controls.component.html',
  styleUrls: ['./game-controls.component.scss'],
  animations: [
    trigger('lastNumber', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('1s', style({ opacity: 1 }))
      ]),
    ])
  ]
})
export class GameControlsComponent implements OnInit, OnDestroy {

  @Input() drawnNumbers: number[];
  @Output() timerElapsed: EventEmitter<void> = new EventEmitter<void>();
  @Output() stopGame: EventEmitter<void> = new EventEmitter<void>();

  public playing: boolean;
  public timerDelay: number = 2000;
  private start: Subject<void> = new Subject<void>();
  private suspend: Subject<void> = new Subject<void>();
  private changeSpeed: Subject<number> = new Subject<number>();
  private subscription: Subscription = new Subscription();

  constructor(private dialog: MatDialog) { }

  ngOnInit(): void {
    this.initiateTimer();
    this.changeSpeed.next(2000);
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
    this.changeSpeed.next(this.timerDelay);
  }

  public forward(): void {
    this.timerDelay -= 500;
    this.changeSpeed.next(this.timerDelay);
  }

  //TODO: Check if I can optimize this piece of code so that the subscription is only
  //      fired in case the user clicks on the start button
  private initiateTimer(): void {
    this.subscription.add(
      this.changeSpeed.pipe(
        switchMap((speed) =>
          timer(0, speed).pipe(
            switchMap(async () => this.timerElapsed.emit()),
            takeUntil(this.suspend),
            repeatWhen(() => this.start)
          )
        )
      ).subscribe()
    );
  }
}
