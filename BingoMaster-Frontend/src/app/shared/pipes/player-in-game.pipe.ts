import { Pipe, PipeTransform } from '@angular/core';
import { PlayerModel } from 'src/api/api';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';

@Pipe({
  name: 'playerInGame',
  pure: false
})
export class PlayerInGamePipe implements PipeTransform {

  constructor(private authenticationService: AuthenticationService) { }

  transform(players: PlayerModel[]): boolean {
    const playerId = this.authenticationService.currentUserValue.playerId;
    return players.some(player => player.id === playerId);
  }

}
