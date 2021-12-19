import { Component, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { NetworkingService } from '../../common/services/networking.service';
import { Subscription } from 'rxjs';


@Component({
  selector: 'app-loading',
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.css']
})

export class LoadingComponent implements OnDestroy {

  isShow = false;
  subscription: Subscription;

  constructor(public networking: NetworkingService, private ChangeDetectorRef: ChangeDetectorRef) {
    // Subscribe to Networking Service
    this.subscription = this.networking.getLoading().subscribe(value => {
      this.isShow = value;
      this.ChangeDetectorRef.detectChanges();
    });
  }

  ngOnDestroy() {
    // Unsubscribe to ensure no memory leaks
    this.subscription.unsubscribe();
  }
 
}
