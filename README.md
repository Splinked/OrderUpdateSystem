# OrderUpdateSystem
 
A work order system for the creation of new orders. If an order is given a “started” status (request to start) then the system locks off the work order
from anyone else. If the work order has already been started then the response for this status change
request is a fail.

If the work order is successfully updated then send a message to the work order
notification service (work order id, status, operator id). 

If the work order is given a “Completed” status then a message needs to be sent to the work order
notification system. A work order can not be completed unless it has started and only if it is in this state.
Only the operator who started the work order can complete the work order.

A work order may be stopped and can be stopped by the operator who started the work order or an
operator with the “supervisor” attribute.

Work orders are stored in various systems and accessed via a repository that manages this. The
repository has a ‘ChangeStatus’ method for changing status and responds with a true or false depending
on if it is successful or not.

If a work order is already in the status of the status change request, then a validation message is
returned.

TODO:
- Web API enpoint to accept JSON/XML requests for order actions
- Authorisation servce for login and enhancments to the existing operator functionality
