import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatChipList } from '@angular/material/chips';
import { Observable } from 'rxjs';
import { map, startWith, take } from 'rxjs/operators';
import { BingoGameDetailModel, BingoGameModel, PlayerGameModel, PlayerModel } from 'src/api/api';
import { BingoGameService } from 'src/app/services/bingo-game.service';
import { PlayerService } from 'src/app/services/player/player.service';

@Component({
  selector: 'game-setup',
  templateUrl: './game-setup.component.html',
  styleUrls: ['./game-setup.component.scss']
})
export class GameSetupComponent implements OnInit {

  @ViewChild('playersInput') playersInput: ElementRef<HTMLInputElement>;
  @ViewChild('playersChipList') playersChipList: MatChipList;

  public gameSetupFormGroup: FormGroup;
  public loading: boolean = false;
  public today: Date = new Date();
  public removable: boolean = true;
  public selectable: boolean = true;
  public separatorKeysCodes: number[] = [ENTER, COMMA];
  public filteredPlayers: Observable<PlayerModel[]>;
  public players: PlayerModel[] = [];
  private allPlayers: PlayerModel[] = [];

  constructor(
    private bingoGameService: BingoGameService,
    private playerService: PlayerService
  ) { }

  ngOnInit(): void {
    this.buildForm();

    this.playerService.getAllPlayers().pipe(
      take(1)
    ).subscribe((players: PlayerModel[]) => {
      this.allPlayers = players;
    });

    this.filteredPlayers = this.gameSetupFormGroup.get('playersInput').valueChanges.pipe(
      startWith(''),
      map(value => typeof value === 'string' ? value : value.name),
      map(name => name ? this.filterPlayers(name) : this.allPlayers.slice()));

    this.gameSetupFormGroup.get('players').statusChanges.subscribe(
      status => this.playersChipList.errorState = status === 'INVALID'
    );
  }

  public createBingoGame(): void {
    this.loading = true;

    const bingoGameModel = this.getBingoGameModel();

    this.bingoGameService.createBingoGame(bingoGameModel)
      .pipe(take(1))
      .subscribe((model: BingoGameModel) => {
        this.loading = false;
        // TODO: Save to backend (send request to API)
      });
  }

  public playerSelected(event: MatAutocompleteSelectedEvent): void {
    const index = this.getPlayerIndex(event.option.value);

    if (index === -1) {
      this.players.push(event.option.value);
      this.gameSetupFormGroup.get('players').setValue(this.players);
    }

    this.playersInput.nativeElement.value = '';
    this.gameSetupFormGroup.get('playersInput').setValue('');
  }

  public removePlayer(player: PlayerModel): void {
    const index = this.getPlayerIndex(player);

    if (index >= 0) {
      this.players.splice(index, 1);
      this.gameSetupFormGroup.get('players').setValue(this.players);
    }
  }

  private getPlayerIndex(player: PlayerModel): number {
    return this.players.findIndex(p => p.id === player.id);
  }

  private maximumAmountOfPlayersValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
    const players = control.get('players');
    const amountOfPlayers = control.get('amountOfPlayers');

    return players && amountOfPlayers && players.value.length > amountOfPlayers.value ? { maximumAmountOfPlayers: true } : null;
  };

  private getBingoGameModel(): BingoGameDetailModel {
    let bingoGameModel = new BingoGameDetailModel();
    bingoGameModel.name = this.gameSetupFormGroup.get('name').value;
    bingoGameModel.size = this.gameSetupFormGroup.get('size').value;
    bingoGameModel.date = this.gameSetupFormGroup.get('date').value;
    bingoGameModel.isPrivateGame = this.gameSetupFormGroup.get('enablePrivateGame').value;
    bingoGameModel.isCenterSquareFree = this.gameSetupFormGroup.get('centerSquareFree').value;
    bingoGameModel.players = this.gamePlayerModels();

    return bingoGameModel;
  }

  private gamePlayerModels(): PlayerGameModel[] {
    return this.gameSetupFormGroup.get('players').value.map((player: PlayerModel) => {
      return {
        name: player.name
      };
    });
  }

  private filterPlayers(value: string): PlayerModel[] {
    const filterValue = value.toLowerCase();

    return this.allPlayers.filter(player => player.name.toLowerCase().includes(filterValue));
  }

  private buildForm(): void {
    this.gameSetupFormGroup = new FormGroup({
      name: new FormControl('', Validators.required),
      size: new FormControl(3, Validators.required),
      date: new FormControl(new Date(), Validators.required),
      amountOfPlayers: new FormControl('', Validators.required),
      playersInput: new FormControl(),
      players: new FormControl(this.players),
      centerSquareFree: new FormControl(''),
      enablePrivateGame: new FormControl('')
    }, [this.maximumAmountOfPlayersValidator]);
  }
}
