import { Pipe, PipeTransform } from '@angular/core';
import { PlayerModel } from 'src/api/api';

@Pipe({
  name: 'playerInGame'
})
export class PlayerInGamePipe implements PipeTransform {

  transform(players: PlayerModel[], id: string): boolean {
    return players.some(player => player.id === id);
  }

}
