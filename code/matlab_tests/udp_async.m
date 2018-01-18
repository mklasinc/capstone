clear all;
clc;
global t;
t = udp('10.225.42.154',9000,'LocalPort', 9001);
t.ReadAsyncMode = 'continuous';
m = 'yooo';
disp(m);
t.BytesAvailableFcnCount = 5;
t.BytesAvailableFcnMode = 'byte';
t.BytesAvailableFcn = @new_message;
fopen(t);

% pause(5);
% close_connection(t);

function new_message(obj,event)
      disp('whaddup');
%       callbackTime = datestr(datenum(event.Data.AbsTime));
%       fprintf(['A ' + event.Type + ' event occurred for ' + obj.Name +  ' at ' + callbackTime '.\n']);
%       disp('the message is');
      global t;
      udp_msg = fscanf(t);
      disp(udp_msg);
       
end

function clear_buffer(udp_obj)
    flushinput(udp_obj);
end

function close_connection(n)
    disp('closing connection!');
    fclose(n);
    delete(n);
end
 
%  while(connection_bool == 0)
%     B = fscanf(t);
%     disp(B);
%         if(contains(B,'quit'))
%             disp("closing down communication");
%             connection_bool = 1;
%             fclose(t);
%             delete(t);
%         end
%    end

% fclose(t);
% delete(t);
% clear all;
% clc;
