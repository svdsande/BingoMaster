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
  }

  public createBingoGame(): void {
    this.loading = true;

    const bingoGameCreationModel = this.getBingoGameCreationModel();

    this.bingoGameService.createBingoGame(bingoGameCreationModel)
      .pipe(take(1))
      .subscribe((model: BingoGameModel) => {
        this.loading = false;
        this.onBingoGameCreated.emit(model);
      });
  }

  public addPlayer(): void {
    this.players.push(new FormControl('', Validators.required));
  }

  public removePlayer(index: number): void {
    this.players.removeAt(index);
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
      players: new FormArray([], Validators.required)
    });
  }
}
