import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { take } from 'rxjs/operators';
import { BingoGameCreationModel, BingoGameModel, PlayerModel } from 'src/api/api';
import { BingoGameService } from 'src/app/services/bingo-game.service';

@Component({
  selector: 'game-setup',
  templateUrl: './game-setup.component.html',
  styleUrls: ['./game-setup.component.scss']
})
export class GameSetupComponent implements OnInit {

  @Output() onBingoGameCreated: EventEmitter<BingoGameModel> = new EventEmitter<BingoGameModel>();
  public gameSetupFormGroup: FormGroup;
  public loading: boolean = false;

  get players() {
    return this.gameSetupFormGroup.get('players') as FormArray;
  }

  constructor(private bingoGameService: BingoGameService) { }

  ngOnInit(): void {
    this.buildForm();

    this.gameSetupFormGroup.get('amountOfPlayers').valueChanges.subscribe((value: number) => {
      if (value > this.players.length) {
        const difference = value - this.players.length;

        for (let i = 0; i < difference; i++) {
          this.players.push(new FormControl('', Validators.required));
        }
      } else if (!value) {
        this.players.clear();
      } else {
        const difference = this.players.length - value;
        for (let i = 0; i < difference; i++) {
          this.players.removeAt(this.players.length - 1);
        }
      }
    });
  }

  public createBingoGame(): void {
    this.loading = true;

    const bingoGameCreationModel = this.getBingoGameCreationModel();
    // const amountOfPlayers = this.gameSetupFormGroup.get('amountOfPlayers').value;

    this.bingoGameService.createBingoGame(bingoGameCreationModel)
      .pipe(take(1))
      .subscribe((model: BingoGameModel) => {
        this.loading = false;
        this.onBingoGameCreated.emit(model);
      });
  }

  private getBingoGameCreationModel(): BingoGameCreationModel {
    let bingoGameCreationModel = new BingoGameCreationModel();
    bingoGameCreationModel.name = this.gameSetupFormGroup.get('name').value;
    bingoGameCreationModel.size = this.gameSetupFormGroup.get('size').value;
    bingoGameCreationModel.players = this.getPlayerModels();

    return bingoGameCreationModel;
  }

  private getPlayerModels(): PlayerModel[] {
    let playerModels: PlayerModel[] = [];

    this.players.controls.forEach(control => {
      const playerModel = new PlayerModel();
      playerModel.name = control.value;

      playerModels.push(playerModel);
    });

    return playerModels;
  }

  private buildForm(): void {
    this.gameSetupFormGroup = new FormGroup({
      name: new FormControl('', Validators.required),
      size: new FormControl(3, Validators.required),
      amountOfPlayers: new FormControl('', Validators.required),
      players: new FormArray([])
    });
  }
}
