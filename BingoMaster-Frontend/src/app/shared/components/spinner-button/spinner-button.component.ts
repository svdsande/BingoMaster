import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'spinner-button',
  templateUrl: './spinner-button.component.html',
  styleUrls: ['./spinner-button.component.scss']
})
export class SpinnerButtonComponent implements OnInit {

  @Input() loading: boolean;

  constructor() { }

  ngOnInit(): void {
  }

}
