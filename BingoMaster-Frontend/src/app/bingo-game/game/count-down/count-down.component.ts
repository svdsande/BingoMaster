import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'count-down',
  templateUrl: './count-down.component.html',
  styleUrls: ['./count-down.component.scss']
})
export class CountDownComponent implements OnInit {

  @Output() done: EventEmitter<void> = new EventEmitter<void>();

  public counter: number = 10;
  public count: number = 10;
  public tickerHeight: string;

  constructor() { }

  ngOnInit(): void {
  }

  public onValueChange(count: number): void {
    this.count = count;
    const difference = this.counter - count;

    if (count <= this.counter) {
      this.tickerHeight = 100 - (difference / this.counter * 100) + '%';
    }

    if (count === 0) {
      setTimeout(() => {
        this.done.emit();
      }, 700);
    }
  }
}
