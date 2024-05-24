// import { NgModule } from '@angular/core';
// import { CommonModule } from '@angular/common';
// import { HttpClientModule } from '@angular/common/http';
// import { routes } from "./app.routes";
// import { BrowserModule } from '@angular/platform-browser';
// import { CardComponent } from './tests/card/card.component';
// import { AllProblemsComponent } from './problems/all-problems/all-problems.component';
// import { AppComponent } from './app.component';
// import { ProblemCardComponent } from './problems/problem-card/problem-card.component';
// import { ProblemComponent } from './problems/problem-component/problem-component.component';
// import { RouterModule } from '@angular/router';

// @NgModule({
//   declarations: [
//     AppComponent,
//     ProblemComponent,
//     ProblemCardComponent,
//     CardComponent,
//     AllProblemsComponent
//   ],
//   imports: [
//     RouterModule.forRoot(routes),
//     CommonModule,
//     HttpClientModule,
//     BrowserModule
//   ],
//   bootstrap: [AppModule]
// })
// export class AppModule { }

// import { NgModule } from '@angular/core';
// import { BrowserModule } from '@angular/platform-browser';
// import { HttpClientModule } from '@angular/common/http';
// import { RouterModule } from '@angular/router';
// import { AppComponent } from './app.component';
// import { routes } from "./app.routes";
// import { ProblemsModule } from './problems/problems.module';

// @NgModule({
//   declarations: [
//     AppComponent
//   ],
//   imports: [
//     BrowserModule,
//     HttpClientModule,
//     RouterModule.forRoot(routes)
//     // ProblemsModule
//   ],
//   bootstrap: [AppComponent]
// })
// export class AppModule { }

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { routes } from './app.routes';
import { ProblemsModule } from './problems/problems.module';
import { ReactiveFormsModule } from '@angular/forms';
import { UserModule } from './user/user.module';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot(routes),
    ProblemsModule,
    ReactiveFormsModule,
    UserModule
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
