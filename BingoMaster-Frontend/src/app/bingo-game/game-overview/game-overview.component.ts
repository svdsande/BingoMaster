import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import * as moment from 'moment';
import { BehaviorSubject } from 'rxjs';
import { map, switchMap, take } from 'rxjs/operators';
import { BingoGameDetailModel, PlayerModel } from 'src/api/api';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { BingoGameService } from 'src/app/services/bingo-game.service';
import { PlayerService } from 'src/app/services/player/player.service';

@Component({
  selector: 'app-game-overview',
  templateUrl: './game-overview.component.html',
  styleUrls: ['./game-overview.component.scss']
})
export class GameOverviewComponent implements OnInit {

  public games: BehaviorSubject<BingoGameDetailModel[]> = new BehaviorSubject<BingoGameDetailModel[]>([]);
  public gameFilterFormGroup: FormGroup;
  private playerId: string;
  private player: PlayerModel;
  private allGames: BingoGameDetailModel[];

  constructor(
    private authenticationService: AuthenticationService,
    private bingoGameService: BingoGameService,
    private playerService: PlayerService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.playerId = this.authenticationService.currentUserValue.playerId;
    this.setPlayer();
    this.getAllPublicGames();
    this.buildForm();

    this.gameFilterFormGroup.get('search').valueChanges.pipe(
      map(searchValue => searchValue ? this.filterGamesByName(searchValue) : this.allGames.slice())
    ).subscribe((filteredGames: BingoGameDetailModel[]) => {
      this.games.next(filteredGames);
    });

    this.gameFilterFormGroup.get('range').valueChanges.pipe(
      map(dateRange => this.filterGamesInDateRange(dateRange))
    ).subscribe((filteredGames: BingoGameDetailModel[]) => {
      this.games.next(filteredGames);
    });
  }

  public join(gameId: string): void {
    this.games.pipe(
      take(1),
      switchMap(games => this.bingoGameService.joinGame(gameId, this.playerId).pipe(
        map(() => games)
      ))
    ).subscribe((games) => {
      this.joinGame(games, gameId);
      this.snackBar.open('Join game', 'Success', {
        duration: 2000
      });
    },
      error => {
        this.snackBar.open('Joining game failed, please refresh and try again', 'Error', {
          duration: 2000
        });
      });
  }

  public leave(gameId: string): void {
    this.games.pipe(
      take(1),
      switchMap(games => this.bingoGameService.leaveGame(gameId, this.playerId).pipe(
        map(() => games)
      ))
    ).subscribe((games) => {
      this.leaveGame(games, gameId);
      this.snackBar.open('Left game', 'Success', {
        duration: 2000
      });
    },
      error => {
        this.snackBar.open('Leaving game failed, please refresh and try again', 'Error', {
          duration: 2000
        });
      });
  }

  private getAllPublicGames(): void {
    this.bingoGameService.getAllPublicGames().pipe(
      take(1)
    ).subscribe((games: BingoGameDetailModel[]) => {
      this.games.next(games);
      this.allGames = games;
    });
  }

  private setPlayer(): void {
    this.playerService.getPlayer(this.playerId).pipe(
      take(1)
    ).subscribe((player: PlayerModel) => {
      this.player = player;
    });
  }

  private buildForm(): void {
    this.gameFilterFormGroup = new FormGroup({
      search: new FormControl(''),
      range: new FormGroup({
        startDate: new FormControl(),
        endDate: new FormControl(),
      })
    });
  }

  private leaveGame(games: BingoGameDetailModel[], gameId: string): void {
    const index = games.findIndex(game => game.id === gameId);

    if (index !== -1) {
      games[index].players = games[index].players.filter(player => player.id !== this.playerId);
    }

    this.games.next(games);
  }

  private joinGame(games: BingoGameDetailModel[], gameId: string): void {
    const index = games.findIndex(game => game.id === gameId);

    if (index !== -1) {
      games[index].players.push(this.player);
    }

    this.games.next(games);
  }

  private filterGamesByName(searchValue: string): BingoGameDetailModel[] {
    const filterValue = searchValue.toLowerCase();

    return this.allGames.filter(game => game.name.toLowerCase().includes(filterValue));
  }

  private filterGamesInDateRange(range: {startDate: moment.Moment, endDate: moment.Moment}): BingoGameDetailModel[] {
    return this.allGames.filter(game => moment(game.date).isBetween(range.startDate, range.endDate));
  }
}
