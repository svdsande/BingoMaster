import { Component, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { take } from 'rxjs/operators';
import { BingoGameDetailModel, BingoGameModel, PlayerGameModel } from 'src/api/api';
import { BingoGameService } from 'src/app/services/bingo-game.service';

@Component({
  selector: 'game-setup',
  templateUrl: './game-setup.component.html',
  styleUrls: ['./game-setup.component.scss']
})
export class GameSetupComponent implements OnInit {

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
        // TODO: Save to backend (send request to API)
      });
  }

  public addPlayer(): void {
    this.players.push(new FormControl('', Validators.required));
  }

  public removePlayer(index: number): void {
    this.players.removeAt(index);
  }

  private getBingoGameCreationModel(): BingoGameDetailModel {
    let bingoGameCreationModel = new BingoGameDetailModel();
    bingoGameCreationModel.name = this.gameSetupFormGroup.get('name').value;
    bingoGameCreationModel.size = this.gameSetupFormGroup.get('size').value;
    bingoGameCreationModel.players = this.getPlayerModels();

    return bingoGameCreationModel;
  }

  private getPlayerModels(): PlayerGameModel[] {
    let playerModels: PlayerGameModel[] = [];

    this.players.controls.forEach(control => {
      const playerModel = new PlayerGameModel();
      playerModel.name = control.value;

      playerModels.push(playerModel);
    });

    return playerModels;
  }

  private buildForm(): void {
    this.gameSetupFormGroup = new FormGroup({
      name: new FormControl('', Validators.required),
      size: new FormControl(3, Validators.required),
      date: new FormControl(new Date(), Validators.required),
      amountOfPlayers: new FormControl('', Validators.required),
      players: new FormArray([], Validators.required)
    });
  }
}
