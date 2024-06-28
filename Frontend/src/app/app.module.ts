import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { routes } from './app.routes';
import { ProblemsModule } from './problems/problems.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { UserModule } from './user/user.module';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { NavbarComponent } from './navbar/navbar.component';

@NgModule({ declarations: [AppComponent, NavbarComponent],
    bootstrap: [AppComponent], imports: [BrowserModule,
        RouterModule.forRoot(routes),
        ProblemsModule,
        ReactiveFormsModule,
        UserModule,
        BrowserAnimationsModule,
        FormsModule,
        ToastrModule.forRoot()], providers: [
        {
            provide: HTTP_INTERCEPTORS,
            useClass: AuthInterceptor,
            multi: true,
        },
        provideHttpClient(withInterceptorsFromDi()),
    ] })
export class AppModule {}
