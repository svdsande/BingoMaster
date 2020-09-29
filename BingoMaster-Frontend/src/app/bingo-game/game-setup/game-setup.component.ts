import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { take } from 'rxjs/operators';
import { BingoCardModel } from 'src/api/api';
import { BingoGameService } from 'src/app/services/bingo-game.service';

@Component({
  selector: 'game-setup',
  templateUrl: './game-setup.component.html',
  styleUrls: ['./game-setup.component.scss']
})
export class GameSetupComponent implements OnInit {

  @Output() onBingoGameCreated: EventEmitter<BingoCardModel[]> = new EventEmitter<BingoCardModel[]>();
  public gameSetupFormGroup: FormGroup;
  public loading: boolean = false;

  constructor(private bingoGameService: BingoGameService) { }

  ngOnInit(): void {
    this.buildForm();
  }

  public createBingoGame(): void {
    this.loading = true;

    const name = this.gameSetupFormGroup.get('name').value;
    const amountOfPlayers = this.gameSetupFormGroup.get('amountOfPlayers').value;
    const size = this.gameSetupFormGroup.get('size').value;

    this.bingoGameService.createBingoGame(name, amountOfPlayers, size)
      .pipe(take(1))
      .subscribe((result) => {
        this.loading = false;
        this.onBingoGameCreated.emit(result);
      });
  }

  private buildForm(): void {
    this.gameSetupFormGroup = new FormGroup({
      name: new FormControl('', Validators.required),
      size: new FormControl(3, Validators.required),
      amountOfPlayers: new FormControl('', Validators.required)
    });
  }
}
