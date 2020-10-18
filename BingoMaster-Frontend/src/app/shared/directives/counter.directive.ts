import { Directive, EventEmitter, Input, OnChanges, OnDestroy, Output, SimpleChanges } from '@angular/core';
import { Subject, Subscription, timer } from 'rxjs';
import { switchMap, take, tap } from 'rxjs/operators';

@Directive({
  selector: '[counter]'
})
export class CounterDirective implements OnChanges, OnDestroy {

  @Input() counter: number;
  @Input() interval: number;
  @Output() value: EventEmitter<number> = new EventEmitter<number>();

  private subscription: Subscription = new Subscription();
  private counterSource: Subject<any> = new Subject<any>();

  constructor() {
    this.subscription.add(this.counterSource.pipe(
        switchMap(({ interval, count }) =>
          timer(0, interval).pipe(
            take(count),
            tap(() => this.value.emit(--count)),
          )
        )
      ).subscribe()
    );
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.counterSource.next({ count: this.counter, interval: this.interval });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
